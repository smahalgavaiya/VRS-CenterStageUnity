using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DisplayObjVal : MonoBehaviour
{
    public GameObject objectToTrack;
    public Component selComp;
    public string selVar;
    public TextMeshProUGUI text;

    //used by custom inspector
    public int compIdx = 0;
    public int varIdx = 0;
    // Start is called before the first frame update
    void Start()
    {
        text = GetComponentInChildren<TextMeshProUGUI>();
    }

    private void OnValidate()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (selComp != null && selVar != "")
        {
            //get field for variables?
            //why is this not saving
            //text.text = selComp.GetType().GetProperty(selVar).GetValue(selComp).ToString();
            if(selComp.GetType().GetProperty(selVar).PropertyType.IsArray)
            {
                string output = "";
                object[] ar = (object[])selComp.GetType().GetProperty(selVar).GetValue(selComp);
                if(ar == null) { text.text = "NULL"; return; }
                for (int i = 0; i< ar.Length; i++ )
                {
                    string val = "NULL";
                    if (ar[i] != null) { val = ar[i].ToString(); }
                    output+= $"[{i}] - {val}\n";
                }
                text.text = output;
            }
            else
            {
                text.text = selComp.GetType().GetProperty(selVar).GetValue(selComp).ToString();
            }
            
        }

    }
}
