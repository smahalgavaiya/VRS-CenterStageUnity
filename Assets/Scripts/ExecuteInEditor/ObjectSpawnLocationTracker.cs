using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[ExecuteInEditMode]
public class ObjectSpawnLocationTracker : MonoBehaviour
{
    public GameObject spawnLocationParent;
    public string currentResourcesFolder;
    ObjectSpawnManager objectSpawnManager;
    private void Start()
    {
        objectSpawnManager = GetComponent<ObjectSpawnManager>();
    }
    // Update is called once per frame
    void Update()
    {

    }

    public void CreateObjectSpawnLocationEmptyObjects()
    {
        SpawnableObject[] spawnableObjects = Resources.LoadAll<SpawnableObject>("SpawnableObjects/" + currentResourcesFolder);

        objectSpawnManager.spawnableObjects = new SpawnableObjectData[spawnableObjects.Length];

        for (int i = 0; i < objectSpawnManager.spawnableObjects.Length; i++)
        {
            objectSpawnManager.spawnableObjects[i] = new SpawnableObjectData();
            objectSpawnManager.spawnableObjects[i].objectName = spawnableObjects[i].name;
            objectSpawnManager.spawnableObjects[i].spawnableObject = spawnableObjects[i];
            objectSpawnManager.spawnableObjects[i].objectPositions = new Transform[spawnableObjects[i].quantityToSpawn];
        }

        if (spawnLocationParent.transform.childCount > 0)
        {
            EditorUtility.DisplayDialog("Objects Already Exist!", "There seem to be child objects on the " + spawnLocationParent.name + " object already, either delete them or continue setting the objects manually", "Ok");
        }

        else
        {
            for (int i = 0; i < objectSpawnManager.spawnableObjects.Length; i++)
            {
                GameObject newObject = new GameObject(objectSpawnManager.spawnableObjects[i].objectName);
                newObject.transform.SetParent(spawnLocationParent.transform);

                for (int j = 0; j < objectSpawnManager.spawnableObjects[i].objectPositions.Length; j++)
                {
                    GameObject newObjectLocationTracker = new GameObject(objectSpawnManager.spawnableObjects[i].objectName + " Location Tracker" + j.ToString());
                    newObjectLocationTracker.transform.SetParent(newObject.transform);
                    objectSpawnManager.spawnableObjects[i].objectPositions[j] = newObjectLocationTracker.transform;

                }
            }
        }

    }
}

