using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

public class LoadSubScene : MonoBehaviour
{
    public AssetReference subScene;
    AsyncOperationHandle<SceneInstance> opHandle;
    private IEnumerator coroutine;
    // Start is called before the first frame update

    private SceneInstance subSceneInstance;

    public UnityEvent<GameObject> onFinishSubScene;


    public void LoadScene()
    {
        coroutine = StartLoad();
        StartCoroutine(coroutine);
    }

    public IEnumerator StartLoad()
    {
        Time.timeScale = 0;
        RobotFileManager.onConfigFinished += GetConfigObject;

        opHandle = Addressables.LoadSceneAsync(subScene.AssetGUID, LoadSceneMode.Additive, true);

        opHandle.Completed += FinishLoad;
        yield return opHandle;

        

        //Enable Camera in sub scene
        
    }

    public void FinishLoad(AsyncOperationHandle<SceneInstance> opHandle)
    {
        if (opHandle.Status == AsyncOperationStatus.Succeeded)
        {
            subSceneInstance = opHandle.Result;
        }
        Time.timeScale = 1;
    }

    public void UnloadScene()
    {

    }

    public void GetConfigObject(GameObject go)
    {
        Scene curScene = SceneManager.GetActiveScene();
        go.transform.parent = null;
        SceneManager.MoveGameObjectToScene(go, curScene);
        Addressables.UnloadSceneAsync(subSceneInstance);
        onFinishSubScene.Invoke(go);
    }
}
