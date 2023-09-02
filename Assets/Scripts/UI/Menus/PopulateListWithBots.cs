using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(OptionListImage))]
public class PopulateListWithBots : MonoBehaviour
{
    OptionListImage listObj;

    // Start is called before the first frame update
    void Start()
    {
        listObj = GetComponent<OptionListImage>();
        if (FieldData.ins == null) { return;  }
        List<BotData> bots = FieldData.bots.GetBotListLocal();
        SetBotList(FieldData.bots);
    }

    public void SetBotList(BuiltInBots bots)
    {
        listObj = GetComponent<OptionListImage>();
        List<BotData> botsList = bots.GetBotListLocal();
        listObj.options = botsList.Select(item => item.name).ToList();
        listObj.images = botsList.Select(item => item.img).ToList();
        listObj.ChangeOption(0);
    }

    public void SetBotList(string course)
    {
        CourseData courseDat = SimManager.GetCourse(course);
        if (courseDat) { SetBotList(courseDat.botsList); }
    }

}
