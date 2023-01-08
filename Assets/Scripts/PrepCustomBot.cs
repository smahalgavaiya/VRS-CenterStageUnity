using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public  class PrepCustomBot: MonoBehaviour
{
    private void Start()
    {
        
    }

    public void PrepBot(string customName)
    {
        spawnAddressablePrefab sp = FindObjectOfType<spawnAddressablePrefab>();
        sp.loadedPrefab.RemoveAllListeners();
        sp.loadedPrefab.AddListener(BotLoaded);
        sp.LoadObjMP(customName);
    }

    public void BotLoaded(GameObject cBot)
    {

    }
}

