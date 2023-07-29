using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SearchService;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.InputSystem;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

public class SceneCombiner : MonoBehaviour
{
    public List<AssetReference> SceneList;
    AsyncOperationHandle<SceneInstance> opHandle;
    private IEnumerator coroutine;
    // Start is called before the first frame update
    void Start()
    {
        coroutine = StartLoad();
        StartCoroutine(coroutine);
    }
    

    public IEnumerator StartLoad()
    {
        foreach (AssetReference asset in SceneList)
        {
            opHandle = Addressables.LoadSceneAsync(asset.AssetGUID,LoadSceneMode.Additive,true);
            yield return opHandle;

            if (opHandle.Status == AsyncOperationStatus.Succeeded)
            {
            }
        }
            
    }


}
