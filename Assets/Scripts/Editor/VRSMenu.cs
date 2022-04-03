using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class VRSMenu : MonoBehaviour
{
    [MenuItem("VRS/Goal Objects/Cube Goal Zone")]
    static void CreateCubeGoal()
    {
        GameObject parentObject;

        if (GameObject.Find("Goals"))
        {
            parentObject = GameObject.Find("Goals");
        }
        else
        {
            GameObject newObject = new GameObject("Goals");
            newObject.transform.parent = GameObject.Find("Props").transform;
            parentObject = newObject;
        }

        string prefabPath = "Assets/Prefabs/Goals/GoalZoneDefault.prefab";
        Object cubeGoal = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);
        GameObject newGoalObject = (GameObject)Instantiate(cubeGoal);
        newGoalObject.transform.parent = parentObject.transform;
        
    }

    [MenuItem("VRS/Goal Objects/Sphere Goal Zone")]
    static void CreateSphereGoal()
    {
        GameObject parentObject;

        if (GameObject.Find("Goals"))
        {
            parentObject = GameObject.Find("Goals");
        }
        else
        {
            GameObject newObject = new GameObject("Goals");
            newObject.transform.parent = GameObject.Find("Props").transform;
            parentObject = newObject;
        }

        string prefabPath = "Assets/Prefabs/Goals/SphereGoalZone.prefab";
        Object object_ = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);
        GameObject newGoalObject = (GameObject)Instantiate(object_);
        newGoalObject.transform.parent = parentObject.transform;
        
    }
    [MenuItem("VRS/Dynamic Objects/Object Type")]
    static void CreateScoringObjectType()
    {
        PopupDialogForScoreObjectTypes newPopup = ScriptableObject.CreateInstance<PopupDialogForScoreObjectTypes>();
        newPopup.CreatePopup();
    }
    [MenuItem("VRS/Dynamic Objects/Object Location")]
    static void CreateScoringObjectLocation()
    {
        PopupForScoringObjectLocation newPopup = ScriptableObject.CreateInstance<PopupForScoringObjectLocation>();
        newPopup.CreatePopup();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}


public class PopupDialogForScoreObjectTypes : EditorWindow
{
    public string assetName = "NewScoreObjectType";
    public string folderPath;
    GameObject objectPrefab = null;

    public void CreatePopup()
    {
        EditorWindow window = EditorWindow.CreateInstance<PopupDialogForScoreObjectTypes>();
        EditorStyles.label.wordWrap = true;
        window.Show();
    }
    private void OnGUI()
    {
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Create a new Score Object Type in the Resources/SpawnableObjects/ScoringObjectTypes folder");
        EditorGUILayout.Space();
        EditorGUILayout.Space();
        EditorGUILayout.Space();

        EditorGUILayout.Space();
        EditorGUILayout.Space();


        string relativePath = "/Resources/SpawnableObjects/";
        folderPath = EditorGUILayout.TextField("Folder:", folderPath);
        if (GUILayout.Button("Get or create path"))
        {
            folderPath = EditorUtility.OpenFolderPanel("Get or create path", "Assets" + relativePath, "");
        }

        assetName = EditorGUILayout.TextField("Name:", assetName);
        objectPrefab = (GameObject)EditorGUI.ObjectField(new Rect(3,55, position.width - 6, 20), "Object Prefab", objectPrefab, typeof(GameObject), false);
        
        if (GUILayout.Button("ok"))
        {
            ObjectType newScoreObjectType = CreateInstance<ObjectType>();
            AssetDatabase.CreateAsset(newScoreObjectType, folderPath + "/" + assetName + ".asset");
            newScoreObjectType.objectPrefab = objectPrefab;
            newScoreObjectType.SetObjectPrefabObjectType();
            this.Close();
        }
        
    }
}
public class PopupForScoringObjectLocation : EditorWindow
{
    public string assetName = "NewScoringObjectLocation";
    string folderPath;
    ObjectType scoreObjectType;
    public void CreatePopup()
    {
        EditorWindow window = EditorWindow.CreateInstance<PopupForScoringObjectLocation>();
        EditorStyles.label.wordWrap = true;
        window.Show();
    }
    private void OnGUI()
    {
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Create a new Score Object Location in the folder of your choice");

        EditorGUILayout.Space();
        EditorGUILayout.Space();
        EditorGUILayout.Space();
        EditorGUILayout.Space();
        EditorGUILayout.Space();

        assetName = EditorGUILayout.TextField("Name:", assetName);

        if (GUILayout.Button("Select folder"))
        {
            folderPath = EditorUtility.OpenFolderPanel("Get or create path", "Assets/Resources/SpawnableObjects", "");
        }

        scoreObjectType = (ObjectType)EditorGUI.ObjectField(new Rect(3,55, position.width - 6, 20), "Score Object Type", scoreObjectType, typeof(ObjectType), false);

        EditorGUILayout.LabelField("Folder Location: " + folderPath);

        if (GUILayout.Button("ok"))
        {
            if (folderPath == null)
            {
                if (EditorUtility.DisplayDialog("No folder selected", "You need to select a folder.", "ok"))
                    return;
            }
            else
            {
                ObjectLocation newScoreObjectLocation = CreateInstance<ObjectLocation>();
                string assetPath = folderPath.Substring(Application.dataPath.Length);
                AssetDatabase.CreateAsset(newScoreObjectLocation, "Assets/" + assetPath + "/" + assetName + ".asset");
                newScoreObjectLocation.objectType = scoreObjectType;
                this.Close();
            }
        }
    }
}
