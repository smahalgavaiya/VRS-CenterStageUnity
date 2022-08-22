using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class debug : MonoBehaviour
{
    public TextMeshProUGUI text;
    private ColorSensor sensor;
    void Start()
    {
        sensor = FindObjectOfType<ColorSensor>();
    }
    void Update()
    {
        text.text = "Color detected : " + sensor.IsSensed();
    }
}
