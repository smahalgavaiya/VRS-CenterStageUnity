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
        List<DataInteractionPair> pairs = RobotConfig.ins.getPart(oldObj);
        DataInteractionPair chosenPair = pairs[0];
        HoverEffect e = newObj.AddComponent<HoverEffect>();
        EventTrigger trig = newObj.AddComponent<EventTrigger>();
        e.BindHover(newObj, selectionMat);
        EventTrigger.Entry entry = new EventTrigger.Entry();

        foreach(DataInteractionPair pair in pairs)
        {
            pair.obj = newObj;
            if(pair.partType == partType)
            {
                chosenPair = pair;
            }
        }
        
        entry.eventID = EventTriggerType.PointerDown;
        entry.callback.AddListener((data) => { ProcessClick((PointerEventData)data, chosenPair); });
        trig.triggers.Add(entry);
    }


    public void ProcessClick(PointerEventData data, DataInteractionPair pair)
    {
        GameObject obj = data.pointerCurrentRaycast.gameObject;


        boundData.SetData(pair.data);
        Debug.Log("Clicked "+obj.name);
    }

    //for joints. on click, stop selection. assign new joint to motor
    //then assign that joint to list, and glow the joint when its hovered over.
    //we also need to add the highlight graphic on top of it(rotate/etc).
    //
}
