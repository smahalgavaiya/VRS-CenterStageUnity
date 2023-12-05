using UnityEngine;
//using AprilTag;
using UnityEngine.UI;
using System;

using System.IO;
using System.Net;
using System.Text;
using Newtonsoft.Json.Linq;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Networking;
using System.Collections;


public class MyDetector :MonoBehaviour {

    [DllImport("__Internal")]
    private static extern void updateAprilTagDetectionData(float posX, float  posY, float  posZ, float pitch, float  roll, float  yaw);

    [DllImport("__Internal")]
    private static extern void updateVisionMessage(string msg, float value);

    [DllImport("__Internal")]
    private static extern void updateCubeDetectionData(string detectionResponse);

    [DllImport("__Internal")]
    private static extern void UploadImage(string url, string formDataName, string fileName, byte[] fileData, int fileDataLength, string otherData);

    


    [SerializeField] int _decimation = 4;
    [SerializeField] float _tagSize = 0.05f;
    [SerializeField] Material _tagMaterial = null;
    [SerializeField] RenderTexture _renderTexture = null; // Add your Render Texture field here.
    [SerializeField] RawImage _webcamPreview = null;
    //[SerializeField] TMP_Text _debugText = null;
    [SerializeField] CameraSwitcher cameraSwitcher;

    [SerializeField]
    Camera cam;


    //AprilTag.TagDetector _detector;
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
        //_detector = new TagDetector(dims.x, dims.y, _decimation);
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
        //_detector.Dispose();
        //_drawer.Dispose();
    }


    Texture2D ResizeTextureMethod(Texture2D sourceTexture, int targetWidth, int targetHeight)
    {
        // Create a new texture with the desired dimensions
        Texture2D resizedTexture = new Texture2D(targetWidth, targetHeight);

        // Calculate the ratio between the source and target dimensions
        float xRatio = (float)sourceTexture.width / targetWidth;
        float yRatio = (float)sourceTexture.height / targetHeight;

        Debug.Log("ResizeTextureInfor"+ targetWidth+" "+ targetHeight+", "+ xRatio+", "+yRatio);

        // Iterate through the pixels of the new texture and sample from the original texture
        for (int y = 0; y < targetHeight; y++)
        {
            for (int x = 0; x < targetWidth; x++)
            {
                // Calculate the corresponding coordinates in the original texture
                int sourceX = Mathf.FloorToInt(x * xRatio);
                int sourceY = Mathf.FloorToInt(y * yRatio);

                // Sample the color from the original texture
                Color color = sourceTexture.GetPixel(sourceX, sourceY);

                // Set the color in the new texture
                resizedTexture.SetPixel(x, y, color);
            }
        }

        // Apply changes to the new texture
        resizedTexture.Apply();

        return resizedTexture;
    }

    IEnumerator PostRequestWithImage()
    {

        cam = _aprilTagCamera;

        RenderTexture mRt = new RenderTexture(_renderTexture.width, _renderTexture.height, _renderTexture.depth, RenderTextureFormat.ARGB32, RenderTextureReadWrite.sRGB);
        mRt.antiAliasing = _renderTexture.antiAliasing;

        float aspect_ratio = (float) mRt.width / mRt.height;

     
        int target_text_width = 340;
        int target_text_height = Convert.ToInt32(target_text_width / aspect_ratio);
        //Debug.Log("mRt Values"+", "+aspect_ratio +", "+ target_text_width +" "+ mRt.width+", "+ mRt.height + ", " + target_text_height);

        var tex = new Texture2D(mRt.width, mRt.height, TextureFormat.ARGB32, false);
        cam.targetTexture = mRt;
        cam.Render();
        RenderTexture.active = mRt;

        tex.ReadPixels(new Rect(0, 0, mRt.width, mRt.height), 0, 0);
        tex.Apply();
        //tex.Reinitialize(target_text_width, target_text_height);

 

        Debug.Log("Requesting Now ...");

        string apiUrl = "http://54.206.117.183:4008/detectCubes";
        apiUrl = "http://0.0.0.0:4008/detectCubes";

        WWWForm form = new WWWForm();
        Texture2D resizedTexture = ResizeTextureMethod(tex, target_text_width, target_text_height);
        form.AddBinaryData("frame", resizedTexture.EncodeToPNG(), "frame.png", "image/png");

        UnityWebRequest www = UnityWebRequest.Post(apiUrl, form);
        //www.SetRequestHeader("Content-Type", "application/octet-stream");

        yield return www.SendWebRequest();


        // Check for errors
        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Error: " + www.error);
            Debug.LogError("Response Code: " + www.responseCode);
            Debug.LogError("Response Text: " + www.downloadHandler.text);
        }
        else
        {
            // Request successful
            Debug.Log("Upload complete!");
            string responseContent = www.downloadHandler.text;
            Debug.Log("Server response (DetectionResult): " + responseContent);

#if UNITY_WEBGL && !UNITY_EDITOR
                try
                {
                    updateCubeDetectionData(responseContent);
                }
                catch {
                    Debug.LogError("Error invoking updateCubeDetectionData");
                }
#endif


#if !UNITY_WEBGL && UNITY_EDITOR

            dynamic jsonData = JObject.Parse(responseContent);
            JArray predictions = jsonData["predictions"];
            if (predictions.Count == 0)
            {
                Debug.Log("DetectionResult: No Cube Detected");
            }
            else
            {
                //Debug.Log("** Detection Results ** ");
                foreach (var bounding_box in predictions)
                {
                    float x = (float)bounding_box["x"];
                    float y = (float)bounding_box["y"];
                    float width = (float)bounding_box["width"];
                    float height = (float)bounding_box["height"];
                    float x1 = x - width / 2;
                    float x2 = x + width / 2;
                    float y1 = y - height / 2;
                    float y2 = y + height / 2;
                    var box = (x1, x2, y1, y2);
                    Debug.Log("DetectionResult: Detected Cube (x1, x2, y1, y2) :: " + box);
                    //updateAprilTagDetectionData(x1, x2, y1, y2, 0.2f, 0.1f);

                try
                {
                
                }
                catch {
                    Debug.LogError("Error invoking updateAprilTagDetectionData");
                }

                }

            }

#endif


        }

    }


    int skip_frames = 0;
    void detect_cubes()
    {
   
        if (skip_frames <= 35)
        {
            skip_frames++;
            return;

        }
        else
        {

            Debug.Log("Detecting Cubes Now...");
            skip_frames = 0;
            StartCoroutine(PostRequestWithImage());

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
            //_detector.ProcessImage(_textureData.GetPixels32(), fov, _tagSize);

            // Detected tag visualization
            /*
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

                Debug.Log("PosX: " +posX+", PosY: "+posY+", PosZ:"+ posZ+", Pitch:"+ pitch+", Roll:"+ roll+", Yaw:"+yaw);



#if UNITY_WEBGL && !UNITY_EDITOR
                try
                {
                    updateAprilTagDetectionData(posX, posY, posZ, pitch, roll, yaw);
                }
                catch {
                    Debug.LogError("Error invoking updateAprilTagDetectionData");
                }
#endif


                break;
            }
            */

            // Profile data output (with 30 frame interval)
            //if(Time.frameCount % 30 == 0)
            //    _debugText.text = _detector.ProfileData.Aggregate
            //      ("Profile (usec)", (c, n) => $"{c}\n{n.name} : {n.time}");
        }
    }
}
