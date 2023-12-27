using FTC_Robot;
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
        ChangeSelectable(true);
    }

    public void StopSelection()
    {
        ChangeSelectable(false);
    }

    public void ChangeSelectable(bool isSelectable)
    {
        List<DataInteractionPair> objs = RobotConfig.ins.getPartsOfType(partType);
        if(objs == null) { return; }
        foreach (DataInteractionPair pair in objs)
        {
            //MouseHover m = obj.AddComponent<MouseHover>();
            GameObject obj = pair.obj;
            
            if(isSelectable)
            {

                ConfigObject conf = (ConfigObject)pair.data;
                conf.AddChangeListener(AddSelectableToNewObj);
                if(obj.GetComponent<HoverEffect>()!= null)
                {
                    break;
                }
                HoverEffect e = obj.AddComponent<HoverEffect>();
                EventTrigger trig = obj.AddComponent<EventTrigger>();
                e.BindHover(obj, selectionMat);
                EventTrigger.Entry entry = new EventTrigger.Entry();
                entry.eventID = EventTriggerType.PointerDown;
                entry.callback.AddListener((data) => { ProcessClick((PointerEventData)data, pair); });
                trig.triggers.Add(entry);
            }
            else
            {
                Destroy(obj.GetComponent<HoverEffect>());
                Destroy(obj.GetComponent<EventTrigger>());
            }
            
        }
    }

    public void AddSelectableToNewObj(GameObject oldObj, GameObject newObj, object dat)
    {
        DataInteractionPair pair = RobotConfig.ins.getPart(oldObj);
        HoverEffect e = newObj.AddComponent<HoverEffect>();
        EventTrigger trig = newObj.AddComponent<EventTrigger>();
        e.BindHover(newObj, selectionMat);
        EventTrigger.Entry entry = new EventTrigger.Entry();
        pair.obj = newObj;
        entry.eventID = EventTriggerType.PointerDown;
        entry.callback.AddListener((data) => { ProcessClick((PointerEventData)data, pair); });
        trig.triggers.Add(entry);
    }


    public void ProcessClick(PointerEventData data, DataInteractionPair pair)
    {
        GameObject obj = data.pointerCurrentRaycast.gameObject;


        boundData.SetData(pair.data);
        Debug.Log("Clicked "+obj.name);
    }
}
