using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempDisableCameras : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        EnableCams(false);
        Invoke("ReEnable", 1);
    }

    public void ReEnable()
    {
        EnableCams();
    }

    // Update is called once per frame
    public void EnableCams(bool enable = true)
    {
        Camera[] cams = GetComponentsInChildren<Camera>(true);
        foreach (Camera c in cams)
        {
            c.gameObject.SetActive(enable);
        }
    }
}
