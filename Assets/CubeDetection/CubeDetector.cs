using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using TMPro;
using System;

public class CubeDetector : MonoBehaviour
{
    [SerializeField] int _decimation = 4;
    [SerializeField] float _tagSize = 0.05f;
    [SerializeField] Material _tagMaterial = null;
    [SerializeField] RenderTexture _renderTexture = null; // Add your Render Texture field here.
    //[SerializeField] RawImage _webcamPreview = null;
    //[SerializeField] TMP_Text _debugText = null;
    [SerializeField] CameraSwitcher cameraSwitcher;


    private Camera _referenceCamera;


    // Use a Texture2D to store the texture data.
    private Texture2D _textureData;



    private void Start()
    {
        cameraSwitcher.OnCameraSwitch += OnCameraSwitched;
    }

    private void StartRendering()
    {
        //var dims = new Vector2Int(_renderTexture.width, _renderTexture.height); // Use the Render Texture dimensions.
        ////dims.x, dims.y, _decimation


        //// Initialize the Texture2D with the dimensions of the Render Texture.
        //_textureData = new Texture2D(_renderTexture.width, _renderTexture.height, TextureFormat.RGBA32, false);
    }

    private void OnCameraSwitched(Camera cameraT)
    {
        if (cameraT.transform.childCount > 0)
        {
            _referenceCamera = cameraT;
            Debug.Log("Camera Tag :: " + _referenceCamera.tag);


            if (true)
            {
                StartRendering();
   
            }
        }
        else
        {

        }
    }

    void OnDestroy()
    {
    
    }

    void LateUpdate()
    {


        Debug.Log("ReadyForCubeDetection");

       
        {
            //// Set the Render Texture as the source for _webcamPreview if needed.
            //if (_webcamPreview != null)
            //{
            //    _webcamPreview.texture = _renderTexture;
            //}

            // Ensure the Render Texture is active for reading.
            //RenderTexture.active = _renderTexture;

            // Read the pixels from the Render Texture into the Texture2D.
            //_textureData.ReadPixels(new Rect(0, 0, _renderTexture.width, _renderTexture.height), 0, 0);
            _textureData.Apply();

            // AprilTag detection
            var fov = _referenceCamera.fieldOfView * Mathf.Deg2Rad;
            //_detector.ProcessImage(_textureData.GetPixels32(), fov, _tagSize);

       
            // Profile data output (with 30 frame interval)
            //if(Time.frameCount % 30 == 0)
            //    _debugText.text = _detector.ProfileData.Aggregate
            //      ("Profile (usec)", (c, n) => $"{c}\n{n.name} : {n.time}");
        }
    }
}