using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetPixelColor : MonoBehaviour
{
    public TeamInventory inventory;

    private Image img;
    // Start is called before the first frame update
    void Start()
    {
        img = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        if(img)
        {
            img.color = inventory.CurrentColor();
        }
    }
}
