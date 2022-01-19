using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[ExecuteInEditMode]
public class ObjectSpawnLocationTracker : MonoBehaviour
{
    public GameObject spawnLocationParent;
    ObjectSpawnManager objectSpawnManager;
    private void Start()
    {
    }
    // Update is called once per frame
    void Update()
    {
        if (objectSpawnManager = null)
        {
            objectSpawnManager = GetComponent<ObjectSpawnManager>();
        }

        else
        {
            return;
        }



    }

    void CreateSpawnObjectTransforms()
    {
        foreach(SpawnableObjectAndPositionParent spawnableObjectAndPositionParent 
            in objectSpawnManager.spawnableObjectAndPositionParents)
        {
            for (int i = 0; i < objectSpawnManager.transform.childCount; i++)
            {

            }

        }
    }
}
