using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.Events;

public class spawnAddressablePrefab : MonoBehaviour
{
    public UnityEvent<GameObject> loadedPrefab;
    public UnityEvent loadError;
    public string defaultLabel = "robot";
    string botname = "";

    // Start is called before the first frame update
    void Start()
    {
        //LoadPrefab("tinybot", "robot");
    }

    void LoadPrefab(string key, string label)
    {
        Addressables.LoadAssetsAsync<GameObject>((IEnumerable)new List<object> { key, label }, null,
            Addressables.MergeMode.Intersection).Completed += PrefabLoaded;

    }

    public void updateBotName(string name)
    {
        botname = name;
    }

    public void LoadObj()
    {
        LoadPrefab(botname, defaultLabel);
    }

    void PrefabLoaded(AsyncOperationHandle<IList<GameObject>> obj)
    {
        if (obj.Result == null) { loadError.Invoke(); }
        else
        {
            loadedPrefab.Invoke(obj.Result[0]);
        }
    }
}
