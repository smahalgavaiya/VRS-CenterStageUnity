using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AprilTag;
using System.Linq;
using UnityEngine.UI;
using TMPro;
using System;

public class MyDetector : MonoBehaviour
{
    [SerializeField] int _decimation = 4;
    [SerializeField] float _tagSize = 0.05f;
    [SerializeField] Material _tagMaterial = null;
    [SerializeField] RenderTexture _renderTexture = null; // Add your Render Texture field here.
    [SerializeField] RawImage _webcamPreview = null;
    //[SerializeField] TMP_Text _debugText = null;
    [SerializeField] CameraSwitcher cameraSwitcher;
    [SerializeField] TMP_Text showText;
    [SerializeField] Color _emissionColor = Color.green;
    string previousTag = "";


    AprilTag.TagDetector _detector;
    TagDrawer _drawer;

    private Camera _referenceCamera;
    private Camera _aprilTagCamera;

    // Use a Texture2D to store the texture data.
    private Texture2D _textureData;

    private bool _isAprilTagCameraActive = false;

    private void Start()
    {
        cameraSwitcher.OnCameraSwitch += OnCameraSwitched;
    }

    private void StartRendering()
    {
        var dims = new Vector2Int(_renderTexture.width, _renderTexture.height); // Use the Render Texture dimensions.
        _detector = new TagDetector(dims.x, dims.y, _decimation);
        _drawer = new TagDrawer(_tagMaterial);

        // Initialize the Texture2D with the dimensions of the Render Texture.
        _textureData = new Texture2D(_renderTexture.width, _renderTexture.height, TextureFormat.RGBA32, false);
    }

    private void OnCameraSwitched(Camera cameraT)
    {
        if (cameraT.transform.childCount > 0)
        {
            _referenceCamera = cameraT;
            _aprilTagCamera = cameraT.transform.GetChild(0).GetComponent<Camera>();

            if (_aprilTagCamera.tag.Equals("AprilTag"))
            {
                StartRendering();
                _isAprilTagCameraActive = true;


            }
        }
        else
        {
            _isAprilTagCameraActive = false;
        }
    }

    void OnDestroy()
    {
        _detector.Dispose();
        _drawer.Dispose();
    }

    void LateUpdate()
    {
        if (_aprilTagCamera == null) return;

        if (_isAprilTagCameraActive || _aprilTagCamera.gameObject.activeInHierarchy)
        {
            // Set the Render Texture as the source for _webcamPreview if needed.
            if (_webcamPreview != null)
            {
                _webcamPreview.texture = _renderTexture;
            }



            // Ensure the Render Texture is active for reading.
            RenderTexture.active = _renderTexture;

            // Read the pixels from the Render Texture into the Texture2D.
            _textureData.ReadPixels(new Rect(0, 0, _renderTexture.width, _renderTexture.height), 0, 0);
            _textureData.Apply();

            // AprilTag detection
            var fov = _referenceCamera.fieldOfView * Mathf.Deg2Rad;
            _detector.ProcessImage(_textureData.GetPixels32(), fov, _tagSize);

            List<string> detectedTagIDs = new List<string>();
            // Detected tag visualization
            foreach (var tag in _detector.DetectedTags)
            {


                _drawer.Draw(tag.ID, tag.Position, tag.Rotation, _tagSize);
                Debug.Log(tag.ID);
                Debug.Log("Tag Position : " + tag.Position);

                detectedTagIDs.Add($" {tag.ID} - Position: {tag.Position}");

                
            }
            showText.text = GetDetectedTagsInfo(detectedTagIDs);
            ToggleEmissionForDetectedTags(detectedTagIDs);

            // Profile data output (with 30 frame interval)
            //if(Time.frameCount % 30 == 0)
            //    _debugText.text = _detector.ProfileData.Aggregate
            //      ("Profile (usec)", (c, n) => $"{c}\n{n.name} : {n.time}");
        }
    }

    string GetDetectedTagsInfo(List<string> detectedTagInfo)
    {
        // Build a string containing information about all detected tags
        if (detectedTagInfo.Count > 0)
        {
            string tagsInfo = "Detected Tags:\n";

            foreach (var tagInfo in detectedTagInfo)
            {
                tagsInfo += $"{tagInfo}\n";
            }

            return tagsInfo;
        }
        else
        {
            return "No tags detected";
        }
    }

    void ToggleEmissionForDetectedTags(List<string> detectedTagInfo)
    {
        // If tags are detected, toggle emission for all detected tags
        if (detectedTagInfo.Count > 0)
        {
            // Iterate through all detected tags
            foreach (var tagInfo in detectedTagInfo)
            {
                string[] parts = tagInfo.Split('-');
                string currentTagID = parts[0].Trim();

                // Iterate through all game objects with the specified tag
                GameObject[] allGameObjects = GameObject.FindGameObjectsWithTag(currentTagID);

                foreach (var gameObject in allGameObjects)
                {
                    Renderer renderer = gameObject.GetComponent<Renderer>();
                    Material material = renderer.material;

                    // Enable emission for the current GameObject
                    material.EnableKeyword("_EMISSION");
                }
            }

            // Iterate through all game objects with the previous tag
            if (!string.IsNullOrEmpty(previousTag) && !detectedTagInfo.Any(tagInfo => tagInfo.Contains(previousTag)))
            {
                GameObject[] previousGameObjects = GameObject.FindGameObjectsWithTag(previousTag);

                foreach (var gameObject in previousGameObjects)
                {
                    Renderer renderer = gameObject.GetComponent<Renderer>();
                    Material material = renderer.material;

                    // Disable emission for previously detected game objects
                    material.DisableKeyword("_EMISSION");
                }
            }

            // Update the previousTag for the next iteration
            previousTag = detectedTagInfo[0].Split('-')[0].Trim(); // Use the first detected tag as the current tag for emission control

            Debug.Log("Previous tag is " + previousTag);
        }
        else
        {
            // No tags detected, turn off emission for all previously detected tags
            if (!string.IsNullOrEmpty(previousTag))
            {
                GameObject[] previousGameObjects = GameObject.FindGameObjectsWithTag(previousTag);

                foreach (var gameObject in previousGameObjects)
                {
                    Renderer renderer = gameObject.GetComponent<Renderer>();
                    Material material = renderer.material;

                    // Disable emission for previously detected game objects
                    material.DisableKeyword("_EMISSION");
                }

                // Reset the previousTag since no tag is currently detected
                previousTag = null;
            }
        }
    }
}