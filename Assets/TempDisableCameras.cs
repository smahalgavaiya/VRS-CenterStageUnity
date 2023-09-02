using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempDisableCameras : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        EnableCams(false);
        Invoke("EnableCams", 1);
    }

    // Update is called once per frame
    public void EnableCams(bool enable = true)
    {
        Camera[] cams = GetComponentsInChildren<Camera>(true);
        foreach (Camera c in cams)
        {
            c.enabled = enable;
        }
    }
}
