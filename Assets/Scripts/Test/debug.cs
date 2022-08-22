using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class debug : MonoBehaviour
{
    public TextMeshProUGUI text;

    public static debug Instance;
    void Start()
    {
        if(Instance == null)
            Instance = this;
        else
            Destroy(this);
    }
    public void SetText(string towrite)
    {
        if(text!= null)
            text.text = towrite;
    }
}
