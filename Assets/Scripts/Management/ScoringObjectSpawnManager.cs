using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[RequireComponent(typeof(ScoringObjectSpawnPositionTracker))]
public class ScoringObjectSpawnManager : MonoBehaviour
{

    public ScoringObjectData[] scoringObjects;

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
public class ScoringObjectData
{
    public string objectName;
    public ScoringObjectLocation scoringObject;
    public Transform[] objectPositions; // The parent transform for a set of locations on the field that
    // should be filled by a set of the spawnableObject prefabs.
}



