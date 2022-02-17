using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ObjectSpawnManager : MonoBehaviour
{

    public SpawnableObjectAndPositionParent[] spawnableObjectAndPositionParents;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

[System.Serializable]
public class SpawnableObjectAndPositionParent
{
    public string objectSetName;
    public SpawnableObject spawnableObject;
    public Transform objectSetPositionParent; // The parent transform for a set of locations on the field that
    // should be filled by a set of the spawnableObject prefabs.
}



