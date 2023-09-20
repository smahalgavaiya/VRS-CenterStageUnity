using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(OptionListImage))]
public class PopulateListWithBots : MonoBehaviour
{
    OptionListImage listObj;

    public UnityEvent onChangeList;

    // Start is called before the first frame update
    void Start()
    {
        listObj = GetComponent<OptionListImage>();
        //if (FieldData.ins == null) { return;  }
        InitBots();
    }

    public void InitBots()
    {
        BuiltInBots bots = SimManager.CurrentCourse.botsList;
        SetBotList(bots);
    }

    public void SetBotList(BuiltInBots bots)
    {
        listObj = GetComponent<OptionListImage>();
        List<BotData> botsList = bots.GetBotListLocal();
        listObj.options = botsList.Select(item => item.name).ToList();
        listObj.images = botsList.Select(item => item.img).ToList();
        if(!PhotonNetwork.IsConnected || PhotonNetwork.IsMasterClient)
        {
            onChangeList.Invoke();
        }
        listObj.ChangeOptionFull(0, false);
    }

    public void SetBotList(string course)
    {
        CourseData courseDat = SimManager.GetCourse(course);
        if (courseDat) { SetBotList(courseDat.botsList); }
    }

}
