using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using System.Linq;
using TMPro;

public class UIDataFill : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Fill(System.Object dataObject, GameObject objToFill)
    {
        UIDataTag[] tags = objToFill.GetComponentsInChildren<UIDataTag>();
        //some kind of interface that will notify when data is changed.
        Dictionary<string,PropertyInfo> props= dataObject.GetType().GetProperties().ToDictionary(p=> p.Name);
        foreach (UIDataTag tag in tags)
        {
            if (props.ContainsKey(tag.tag)) 
            {
                TextMeshProUGUI text = tag.gameObject.GetComponent<TextMeshProUGUI>();
                text.text = props[tag.tag].GetValue(dataObject).ToString();
            }
        }
    }
}
