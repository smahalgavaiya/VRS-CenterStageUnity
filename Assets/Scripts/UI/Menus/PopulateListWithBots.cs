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
        List<BotData> bots = BuiltInBots.GetBotList();
        listObj.options = bots.Select(item => item.name).ToList();
        listObj.images = bots.Select(item => item.img).ToList();
        listObj.ChangeOption(0);
    }

}
