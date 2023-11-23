using UnityEngine;
using AprilTag;
using UnityEngine.UI;
using System;

using System.IO;
using System.Net;
using System.Text;
using Newtonsoft.Json.Linq;
using System.Runtime.InteropServices;

public class MyDetector :MonoBehaviour {

    [DllImport("__Internal")]
    private static extern void updateAprilTagDetectionData(float posX, float  posY, float  posZ, float pitch, float  roll, float  yaw);


    [SerializeField] int _decimation = 4;
    [SerializeField] float _tagSize = 0.05f;
    [SerializeField] Material _tagMaterial = null;
    [SerializeField] RenderTexture _renderTexture = null; // Add your Render Texture field here.
    [SerializeField] RawImage _webcamPreview = null;
    //[SerializeField] TMP_Text _debugText = null;
    [SerializeField] CameraSwitcher cameraSwitcher;

    AprilTag.TagDetector _detector;
    //TagDrawer _drawer;

    private Camera _referenceCamera;
    private Camera _aprilTagCamera;

    // Use a Texture2D to store the texture data.
    private Texture2D _textureData;

    private bool _isAprilTagCameraActive = false;

    private void Start() {
        cameraSwitcher.OnCameraSwitch += OnCameraSwitched;
    }

    private void StartRendering() {
        var dims = new Vector2Int(_renderTexture.width, _renderTexture.height); // Use the Render Texture dimensions.
        _detector = new TagDetector(dims.x, dims.y, _decimation);
        //_drawer = new TagDrawer(_tagMaterial);

        // Initialize the Texture2D with the dimensions of the Render Texture.
        _textureData = new Texture2D(_renderTexture.width, _renderTexture.height, TextureFormat.RGBA32, false);
    }

    private void OnCameraSwitched(Camera cameraT) {
        if(cameraT.transform.childCount > 0) {
            _referenceCamera = cameraT;
            _aprilTagCamera = cameraT.transform.GetChild(0).GetComponent<Camera>();

            if(_aprilTagCamera.tag.Equals("AprilTag")) {
                StartRendering();
                _isAprilTagCameraActive = true;
            }
        }
        else {
            _isAprilTagCameraActive = false;
        }
    }

    void OnDestroy() {
        _detector.Dispose();
        //_drawer.Dispose();
    }

    bool isProcessing = false;

    void DrawRect()
    {

        Vector3 rectanglePosition = new Vector3(0f, 0f, 0f);
        float width_ = 5f;
        float height_ = 5f;
        Color rectangleColor = Color.blue;
        Gizmos.color = rectangleColor;
        Material rectangleMaterial = new Material(Shader.Find("Standard"));
        rectangleMaterial.color = Color.cyan;


        Mesh mesh = new Mesh();

        // Define vertices for a rectangle
        Vector3[] vertices = new Vector3[]
        {
            new Vector3(-width_ * 0.5f, -height_ * 0.5f, 0f),
            new Vector3(width_ * 0.5f, -height_ * 0.5f, 0f),
            new Vector3(width_ * 0.5f, height_ * 0.5f, 0f),
            new Vector3(-width_ * 0.5f, height_ * 0.5f, 0f),
        };

        // Define triangles
        int[] triangles = new int[] { 0, 1, 2, 0, 2, 3 };

        // Set vertices and triangles
        mesh.vertices = vertices;
        mesh.triangles = triangles;

        // Draw the mesh
        Graphics.DrawMesh(mesh, Matrix4x4.TRS(rectanglePosition, Quaternion.identity, Vector3.one), rectangleMaterial, 0);


    }
    void processImageForCubesDetectionAsync()
    {
        if (isProcessing)
        {
            //Debug.Log("Locked!");
            return;
        }

        isProcessing = true;

        //byte[] imageArray = System.IO.File.ReadAllBytes(@"YOUR_IMAGE.jpg");
        byte[] imageArray = _textureData.EncodeToJPG();
        string encoded = Convert.ToBase64String(imageArray);
        byte[] data = Encoding.ASCII.GetBytes(encoded);

        // Construct the URL
        string uploadURL = "https://detect.roboflow.com/cube-detection-orbbf/1?api_key=1lPOuGzdqyHSz7qqWycT";


        // Service Request Config
        ServicePointManager.Expect100Continue = true;
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

        // Configure Request
        WebRequest request = WebRequest.Create(uploadURL);
        request.Method = "POST";
        request.ContentType = "application/x-www-form-urlencoded";
        request.ContentLength = data.Length;


        //Debug.Log("Sending Image");
        // Write Data
        using (Stream stream = request.GetRequestStream())
        {
            stream.Write(data, 0, data.Length);
        }


        //Debug.Log("Processing Stream");
        // Get Response
        string responseContent = null;
        using (WebResponse response = request.GetResponse())
        {
            using (Stream stream = response.GetResponseStream())
            {
                using (StreamReader sr99 = new StreamReader(stream))
                {
                    responseContent = sr99.ReadToEnd();
                }
            }
        }

        dynamic jsonData = JObject.Parse(responseContent);
        JArray predictions = jsonData["predictions"];
        if (predictions.Count == 0)
        {
            Debug.Log("No Cube Detected");
        }
        else
        {
            //Debug.Log("** Detection Results ** ");
            foreach (var bounding_box in predictions)
            {
                float x = (float) bounding_box["x"];
                float y = (float)bounding_box["y"];
                float width = (float)bounding_box["width"];
                float height = (float)bounding_box["height"];
                var x1 = x - width / 2;
                var x2 = x + width / 2;
                var y1 = y - height / 2;
                var y2 = y + height / 2;
                var box = (x1, x2, y1, y2);
                Debug.Log("Detected Cube (x1, x2, y1, y2) :: " + box);
            }

        }

        //Debug.Log("Detection Response"+responseContent);
        DrawRect();
        isProcessing = false;
        
    }






    int skip_frames = 0;
    void detect_cubes()
    {
        //processImageForCubesDetectionAsync();

        if (skip_frames <= 30)
        {
            skip_frames++;
            return;

        }
        else
        {
            //Debug.Log("Detecting Cubes Now...");
            skip_frames = 0;
            processImageForCubesDetectionAsync();
        }

    } 

    void LateUpdate() {
        if(_aprilTagCamera == null) return;

        if(_isAprilTagCameraActive || _aprilTagCamera.gameObject.activeInHierarchy) {

     

            // Set the Render Texture as the source for _webcamPreview if needed.
            if (_webcamPreview != null) {
                _webcamPreview.texture = _renderTexture;
            }

            // Ensure the Render Texture is active for reading.
            RenderTexture.active = _renderTexture;

            // Read the pixels from the Render Texture into the Texture2D.
            _textureData.ReadPixels(new Rect(0, 0, _renderTexture.width, _renderTexture.height), 0, 0);
            _textureData.Apply();


            detect_cubes();
            // AprilTag detection
            var fov = _referenceCamera.fieldOfView * Mathf.Deg2Rad;
            _detector.ProcessImage(_textureData.GetPixels32(), fov, _tagSize);

            // Detected tag visualization
            foreach(var tag in _detector.DetectedTags) {
                //_drawer.Draw(tag.ID, tag.Position, tag.Rotation, _tagSize);
                Debug.Log(tag.Position);
                float posX = tag.Position.x;
                float posY = tag.Position.y;
                float posZ = tag.Position.z;

                Vector3 eulerAngles = tag.Rotation.eulerAngles;

                float pitch = eulerAngles.x;
                float roll = eulerAngles.y;
                float yaw = eulerAngles.z;



#if UNITY_WEBGL && !UNITY_EDITOR
                try
                {
                    updateAprilTagDetectionData: function (posX, posY, posZ, pitch, roll, yaw);
                }
                catch {
                    Debug.LogError("Error invoking updateAprilTagDetectionData");
                }
#endif


            }

            // Profile data output (with 30 frame interval)
            //if(Time.frameCount % 30 == 0)
            //    _debugText.text = _detector.ProfileData.Aggregate
            //      ("Profile (usec)", (c, n) => $"{c}\n{n.name} : {n.time}");
        }
    }
}
