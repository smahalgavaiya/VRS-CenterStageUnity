using System.Collections;
using System.Collections.Generic;
using UnityEditor.AddressableAssets;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.AddressableAssets.ResourceLocators;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceLocations;

public class LoadCatalog : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine("StartLoad");
    }

    public IEnumerator StartLoad()
    {
        //Load a catalog and automatically release the operation handle.
        AsyncOperationHandle<IResourceLocator> handle = Addressables.LoadContentCatalogAsync("http://localhost/testcatalog.json", true);
        yield return handle;

        var loadResourceLocationsHandle
        = Addressables.LoadResourceLocationsAsync("robot", typeof(GameObject));

        if (!loadResourceLocationsHandle.IsDone)
            yield return loadResourceLocationsHandle;

        IResourceLocator log = handle.Result;
        List<string> str = AddressableAssetSettingsDefaultObject.Settings.GetLabels();
        foreach (IResourceLocation location in loadResourceLocationsHandle.Result)
        {
            Debug.Log(location);
        }

        Debug.Log(handle);
    }

}
