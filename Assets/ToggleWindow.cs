using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleWindow : MonoBehaviour
{
    public GameObject window;
    // Start is called before the first frame update
    public void Toggle()
    {
        window.SetActive(!window.activeInHierarchy);
    }
}
