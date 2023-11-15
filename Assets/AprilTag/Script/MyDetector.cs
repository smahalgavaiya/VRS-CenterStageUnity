using UnityEngine;
using AprilTag;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class MyDetector : MonoBehaviour {
    [SerializeField] private TagDetectorConfiguration tagDetectorConfig;
    [SerializeField] private CameraSwitcher cameraSwitcher;
    [SerializeField] private RawImage webcamPreview;
    [SerializeField] private TMP_Text showText;
    [SerializeField] private CanvasGroup parentCanvasGroup;

    private TagDetector _detector;
    private Camera _referenceCamera;
    private Camera _aprilTagCamera;
    private Texture2D _textureData;
    private HashSet<int> _previousDetectedTags = new HashSet<int>();
    private bool _isAprilTagCameraActive = false;

    private void Start() {
        cameraSwitcher.OnCameraSwitch += OnCameraSwitched;
    }

    private void StartRendering() {
        var dims = new Vector2Int(tagDetectorConfig.RenderTexture.width, tagDetectorConfig.RenderTexture.height);
        _detector = new TagDetector(dims.x, dims.y, tagDetectorConfig.Decimation);
        _textureData = new Texture2D(dims.x, dims.y, TextureFormat.RGBA32, false);
    }

    private void OnCameraSwitched(Camera cameraT) {
        HandleCameraSwitch(cameraT);
    }

    void OnDestroy() {
        DisposeDetector();
    }

    void LateUpdate() {
        if (_isAprilTagCameraActive) {
            UpdateTagDetection();
        }
    }

    private void HandleCameraSwitch(Camera cameraT) {
        if(cameraT.transform.childCount > 0) {
            _referenceCamera = cameraT;
            _aprilTagCamera = cameraT.transform.GetChild(0).GetComponent<Camera>();

            if(_aprilTagCamera.tag.Equals("AprilTag")) {
                StartRendering();
                _isAprilTagCameraActive = true;
                parentCanvasGroup.alpha = 1;
            }
        }
        else {
            _isAprilTagCameraActive = false;
            parentCanvasGroup.alpha = 0;
        }
    }

    private void DisposeDetector() {
        _detector.Dispose();
    }

    private void UpdateTagDetection() {
        if(_aprilTagCamera == null) return;

        if(!_aprilTagCamera.gameObject.activeInHierarchy) return;
    
        // Set the Render Texture as the source for _webcamPreview if needed.
        if(webcamPreview != null) {
            webcamPreview.texture = tagDetectorConfig.RenderTexture;
        }

        // Ensure the Render Texture is active for reading.
        RenderTexture.active = tagDetectorConfig.RenderTexture;
        
        // Read the pixels from the Render Texture into the Texture2D.
        _textureData.ReadPixels(new Rect(0, 0, tagDetectorConfig.RenderTexture.width, tagDetectorConfig.RenderTexture.height), 0, 0);
        _textureData.Apply();

        // AprilTag detection
        var fov = _referenceCamera.fieldOfView * Mathf.Deg2Rad;
        _detector.ProcessImage(_textureData.GetPixels32(), fov, tagDetectorConfig.TagSize);
        
        HashSet<int> currentDetectedTags = new HashSet<int>();

        // Detected tag visualization
        foreach (var tag in _detector.DetectedTags) {

            currentDetectedTags.Add(tag.ID);
            Debug.Log(tag.ID);
            showText.text = "April Tag Detected: " + tag.ID;

            GameObject go = GameObject.FindGameObjectWithTag(tag.ID.ToString());

            if (go != null)
            {
                Renderer renderer = go.GetComponent<Renderer>();
                Material material = renderer.material;
                material.EnableKeyword("_EMISSION");
                currentDetectedTags.Add(tag.ID);
            }
            else
            {
                Debug.LogError("GameObject with tag 'tag.ID' not found.");
            }
        }

        // Turn off emission for tags that were previously detected but not currently
        foreach (var prevTag in _previousDetectedTags) {
            if (!currentDetectedTags.Contains(prevTag)) {
                GameObject go = GameObject.FindGameObjectWithTag(prevTag.ToString());
                if (go != null) {
                    Renderer renderer = go.GetComponent<Renderer>();
                    Material material = renderer.material;
                    material.DisableKeyword("_EMISSION");
                }
            }
        }

        // Update the previous detected tags list for the next frame
        _previousDetectedTags = currentDetectedTags;
    }

    // Additional private helper methods
}