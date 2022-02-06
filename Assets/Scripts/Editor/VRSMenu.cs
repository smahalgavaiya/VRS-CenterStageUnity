using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class VRSMenu : MonoBehaviour
{

    [MenuItem("VRS/Create/Goal Objects/Cube Goal Zone")]
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

    [MenuItem("VRS/Create/Goal Objects/Sphere Goal Zone")]
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

    // Update is called once per frame
    void Update()
    {
        
    }
}
