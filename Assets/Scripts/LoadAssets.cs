using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleFileBrowser;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UI;
using UnityEngine.AddressableAssets.ResourceLocators;

public class LoadAssets : MonoBehaviour
{
	public delegate void GetFile();

	public GetFile ReturnTheFile;

	[SerializeField]
	GameObject importedRobot;

	AsyncOperationHandle<GameObject> opCube;

	[SerializeField]
	GameObject dynamicObjectParent, jumpButton;

	public string Path_ { get; set; }
	// Start is called before the first frame update
	void Start()
	{
	}
	public void LoadFile()
	{
		// Add a new quick link to the browser (optional) (returns true if quick link is added successfully)
		// It is sufficient to add a quick link just once
		// Name: Users
		// Path: C:\Users
		// Icon: default (folder icon)
		FileBrowser.AddQuickLink( "Users", "C:\\Users", null );

		// Show a save file dialog 
		// onSuccess event: not registered (which means this dialog is pretty useless)
		// onCancel event: not registered
		// Save file/folder: file, Allow multiple selection: false
		// Initial path: "C:\", Initial filename: "Screenshot.png"
		// Title: "Save As", Submit button text: "Save"
		// FileBrowser.ShowSaveDialog( null, null, FileBrowser.PickMode.Files, false, "C:\\", "Screenshot.png", "Save As", "Save" );

		// Show a select folder dialog 
		// onSuccess event: print the selected folder's path
		// onCancel event: print "Canceled"
		// Load file/folder: folder, Allow multiple selection: false
		// Initial path: default (Documents), Initial filename: empty
		// Title: "Select Folder", Submit button text: "Select"
		// FileBrowser.ShowLoadDialog( ( paths ) => { Debug.Log( "Selected: " + paths[0] ); },
		//						   () => { Debug.Log( "Canceled" ); },
		//						   FileBrowser.PickMode.Folders, false, null, null, "Select Folder", "Select" );

		// Coroutine example
		StartCoroutine( ShowLoadDialogCoroutine() );
	}

	IEnumerator ShowLoadDialogCoroutine()
	{
		// Show a load file dialog and wait for a response from user
		// Load file/folder: both, Allow multiple selection: true
		// Initial path: default (Documents), Initial filename: empty
		// Title: "Load File", Submit button text: "Load"
		yield return FileBrowser.WaitForLoadDialog( FileBrowser.PickMode.FilesAndFolders, true, "H:\\Github\\VRSConstructorKit\\Build\\Robot Simulator_Data\\StreamingAssets\\aa\\StandaloneWindows64", null, "Load Files and Folders", "Load" );

		// Dialog is closed
		// Print whether the user has selected some files/folders or cancelled the operation (FileBrowser.Success)

		if( FileBrowser.Success )
		{
			Path_ = FileBrowser.Result[0];
			Debug.Log(Path_);

			Instantiate(importedRobot);
			
			//ReturnTheFile();
		}
	}

	IEnumerator InstantiateObject()
    {

        opCube = Addressables.LoadAssetAsync<GameObject>("Cube");
        yield return opCube;

        //     opHop = Addressables.LoadAssetAsync<TestDrive>("Hop");
        //     yield return opHop;

        //     if (opHop.Status == AsyncOperationStatus.Succeeded)
        //     {
        //         newTestDrive = opHop.Result;

        //jumpButton.GetComponent<Button>().onClick.AddListener(newTestDrive.Jump);

        //         opCube = Addressables.LoadAssetAsync<GameObject>("Cube");
        //         yield return opCube;
        //     }

        if (opCube.Status == AsyncOperationStatus.Succeeded)
        {
            GameObject newObj = opCube.Result;
            Instantiate(newObj, dynamicObjectParent.transform);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
