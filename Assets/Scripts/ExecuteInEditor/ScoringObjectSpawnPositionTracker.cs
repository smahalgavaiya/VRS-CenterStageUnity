using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[ExecuteInEditMode]
public class ScoringObjectSpawnPositionTracker : MonoBehaviour
{
    public GameObject spawnLocationParent;
    public string currentResourcesFolder;
    ScoringObjectSpawnManager scoringObjectSpawnManager;
    private void OnEnable()
    {
        scoringObjectSpawnManager = GetComponent<ScoringObjectSpawnManager>();
    }
    // Update is called once per frame
    void Update()
    {

    }

    public void CreateObjectSpawnLocationEmptyObjects()
    {
        ScoringObject[] spawnableObjects = Resources.LoadAll<ScoringObject>("SpawnableObjects/" + currentResourcesFolder);

        if (scoringObjectSpawnManager == null)
            scoringObjectSpawnManager = GetComponent<ScoringObjectSpawnManager>();

        scoringObjectSpawnManager.scoringObjects = new ScoringObjectData[spawnableObjects.Length];

        for (int i = 0; i < scoringObjectSpawnManager.scoringObjects.Length; i++)
        {
            scoringObjectSpawnManager.scoringObjects[i] = new ScoringObjectData();
            scoringObjectSpawnManager.scoringObjects[i].objectName = spawnableObjects[i].name;
            scoringObjectSpawnManager.scoringObjects[i].scoringObject = spawnableObjects[i];
            scoringObjectSpawnManager.scoringObjects[i].objectPositions = new Transform[spawnableObjects[i].quantityToSpawn];
        }

        if (spawnLocationParent.transform.childCount > 0)
        {
            EditorUtility.DisplayDialog("Objects Already Exist!", "There seem to be child objects on the " + spawnLocationParent.name + " object already, either delete them or continue setting the objects manually", "Ok");
        }

        else
        {
            for (int i = 0; i < scoringObjectSpawnManager.scoringObjects.Length; i++)
            {
                GameObject newObject = new GameObject(scoringObjectSpawnManager.scoringObjects[i].objectName);
                newObject.transform.SetParent(spawnLocationParent.transform);

                for (int j = 0; j < scoringObjectSpawnManager.scoringObjects[i].objectPositions.Length; j++)
                {
                    GameObject newObjectLocationTracker = new GameObject(scoringObjectSpawnManager.scoringObjects[i].objectName + " Location Tracker" + j.ToString());

                    // Try to set the position of the tracker object
                    if (scoringObjectSpawnManager.scoringObjects[i].scoringObject.pointPositions[j] != null)
                        newObjectLocationTracker.transform.position = 
                            scoringObjectSpawnManager.scoringObjects[i].scoringObject.pointPositions[j];

                    ScoreObjectLink scoreObjectLink = newObjectLocationTracker.AddComponent<ScoreObjectLink>();
                    scoreObjectLink.ScoringObject_ = scoringObjectSpawnManager.scoringObjects[i].scoringObject;
                    scoreObjectLink.SpawnType_ = scoringObjectSpawnManager.scoringObjects[i].scoringObject.spawnType;
                    scoreObjectLink.Index = j;


                    newObjectLocationTracker.transform.SetParent(newObject.transform);
                    scoringObjectSpawnManager.scoringObjects[i].objectPositions[j] = newObjectLocationTracker.transform;

                }
            }
        }

    }
}

