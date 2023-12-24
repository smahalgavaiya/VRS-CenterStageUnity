using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using VFTC.Wizard;

public class MakePartsSelectable : MonoBehaviour
{
    public Material selectionMat;
    public UIBoundData boundData;
    public PartType partType;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void MakeSelectable()
    {
        Debug.Log("making selectable");
        List<GameObject> objs = RobotConfig.ins.getPartsOfType(partType);

        foreach (GameObject obj in objs)
        {
            //MouseHover m = obj.AddComponent<MouseHover>();
            
            HoverEffect e = obj.AddComponent<HoverEffect>();
            //m.enterEvent += () => e.Hover(obj, true);
            //m.exitEvent += () => e.Hover(obj, false);
            EventTrigger trig = obj.AddComponent<EventTrigger>();
            e.BindHover(obj, selectionMat);
            EventTrigger.Entry entry = new EventTrigger.Entry();
            entry.eventID = EventTriggerType.PointerDown;
            entry.callback.AddListener((data) => { ProcessClick((PointerEventData)data); });
            trig.triggers.Add(entry);
        }
    }

    public void ProcessClick(PointerEventData data)
    {
        GameObject obj = data.lastPress;
        WheelScript wheel = obj.GetComponent<WheelScript>();
        boundData.SetData(wheel);
        Debug.Log("Clicked "+obj.name);
    }
}
