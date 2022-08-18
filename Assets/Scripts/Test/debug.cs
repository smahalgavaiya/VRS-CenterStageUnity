using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class debug : MonoBehaviour
{
    public TextMeshProUGUI text;
    void Update()
    {
        text.text = "Gamemode: " + FindObjectOfType<FieldManager>().mode;
    }
}
