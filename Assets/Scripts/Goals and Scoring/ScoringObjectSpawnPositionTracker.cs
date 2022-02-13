using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

[ExecuteInEditMode]
public class ScoringObjectSpawnPositionTracker : MonoBehaviour
{
    public GameObject spawnLocationParent;
    public string resourcesFolder;
    ScoringObjectSpawnManager scoringObjectSpawnManager;
    public MaterialIndex materialIndex;

    // These are used when iterating through the various scoring object locations
    ScoringObjectLocation currentScoringObjectLocation;
    SpawnType currentSpawnType;

    public ScoringObjectSpawnManager ScoringObjectSpawnManager_ { get { return scoringObjectSpawnManager; } }
    private void OnEnable()
    {
        scoringObjectSpawnManager = GetComponent<ScoringObjectSpawnManager>();
    }
    // Update is called once per frame
    void Update()
    {

    }

    // This creates the position tracker objects based on the different 
    // scoring object locations we define in Resources
    //
    // (It is activated by a button, which is implemented by a Custom Editor for 
    // this class)
    public void CreateObjectSpawnLocationEmptyObjects()
    {
        // Get the resources from the designated folder. (The folder name can be
        // designated in the editor for any object with this class-- typically 
        // the @ScoreObjectSpawnManager.)
        ScoringObjectLocation[] scoringObjectLocations = Resources.LoadAll<ScoringObjectLocation>("SpawnableObjects/" + resourcesFolder);

        // If this reference wasn't previously defined, define it now 
        if (scoringObjectSpawnManager == null)
            scoringObjectSpawnManager = GetComponent<ScoringObjectSpawnManager>();

        // Create an array of scoring objects based on the number of objects in the 
        // Resources folder.
        scoringObjectSpawnManager.scoringObjects = new ScoringObjectData[scoringObjectLocations.Length];

        // Fill the previous array with data from the Resources folder
        for (int i = 0; i < scoringObjectSpawnManager.scoringObjects.Length; i++)
        {
            scoringObjectSpawnManager.scoringObjects[i] = new ScoringObjectData();
            scoringObjectSpawnManager.scoringObjects[i].objectName = scoringObjectLocations[i].name;
            scoringObjectSpawnManager.scoringObjects[i].scoringObject = scoringObjectLocations[i];

            // Determine how many tracker objects to spawn per scoring object location, based on Spawn Type
            if (scoringObjectLocations[i].spawnType == SpawnType.AtSpecificPoints)
                scoringObjectSpawnManager.scoringObjects[i].objectPositions = new Transform[scoringObjectLocations[i].quantityToSpawn];
            else if (scoringObjectLocations[i].spawnType == SpawnType.RandomOverMultiplePoints)
                scoringObjectSpawnManager.scoringObjects[i].objectPositions = new Transform[scoringObjectLocations[i].numberOfPotentialPoints];
            else 
                scoringObjectSpawnManager.scoringObjects[i].objectPositions = new Transform[1];

        }

        // This class automatically generates empty GameObjects that serve as the
        // position trackers for score object spawn locations. If the designated
        // Parent object for those trackers isn't empty of children, this error will throw.
        if (spawnLocationParent.transform.childCount > 0)
        {
            EditorUtility.DisplayDialog("Objects Already Exist!", "There seem to be child objects on the " + spawnLocationParent.name + " object already, either delete them or continue setting the objects manually", "Ok");
        }

        // Otherwise, create the empty tracker objects...
        else
        {
            ScoringObjectData currentScoringObjectData; // The internal data used to track scoring objects

            for (int i = 0; i < scoringObjectSpawnManager.scoringObjects.Length; i++)
            {
                currentScoringObjectData = scoringObjectSpawnManager.scoringObjects[i];

                // First create the parent for the empty tracker objects, which should
                // have the same name as the Scoring Object Locations from Resources
                GameObject newObject = 
                    new GameObject(currentScoringObjectData.objectName);
                newObject.transform.SetParent(spawnLocationParent.transform);

                // Now create the tracker objects themselves, based on which type of
                // Scoring Object Location they are (zone, point, random at multiple points, stacked at point)

                currentScoringObjectLocation = scoringObjectSpawnManager.scoringObjects[i].scoringObject;
                currentSpawnType = currentScoringObjectLocation.spawnType;

                switch (currentSpawnType)
                {
                    case SpawnType.AtSpecificPoints:
                        CreateTrackersAtSpecificPoints(currentScoringObjectData, newObject);
                        break;
                    case SpawnType.RandomOverArea:
                        CreateZones();
                        break;
                    case SpawnType.RandomOverMultiplePoints:
                        CreatePotentialPoints();
                        break;
                    case SpawnType.StackedAtPoint:
                        CreateSingleStackedPoint();
                        break;
                }
            }
        }

    }

    private void CreateSingleStackedPoint()
    {
        throw new NotImplementedException();
    }

    private void CreatePotentialPoints()
    {
        throw new NotImplementedException();
    }

    private void CreateZones()
    {
        throw new NotImplementedException();
    }

    void CreateTrackersAtSpecificPoints(ScoringObjectData currentScoringObjectData, GameObject newObject)
    {
        // the quantity defined
        // in the Resource Score Object Location quantityToSpawn, which propogates
        // to the objectPositions of the array above. 
        // 
        // We use the length of that array instead of quantityToSpawn just to 
        // be careful that we don't exceed the size of the array.

        for (int j = 0; j < currentScoringObjectData.objectPositions.Length; j++)
        {
            GameObject newObjectLocationTracker = new GameObject(currentScoringObjectData.objectName + " Location Tracker" + (j+1).ToString());

            // Create a "ghost" image of the prefab
            AssignTrackerMaterialAndSetZeroPosition(newObjectLocationTracker, currentScoringObjectData.scoringObject.scoreObjectType.objectPrefab);

            // Try to set the position of the tracker object (if it was previously set and saved to the 
            // Scoring Object Location asset)
            if (j < currentScoringObjectData.scoringObject.pointPositions.Count &&
                currentScoringObjectData.scoringObject.pointPositions[j] != null)
                newObjectLocationTracker.transform.position = 
                    currentScoringObjectData.scoringObject.pointPositions[j];

            // Add the ScoreObjectLink so when we move it around the position data
            // propogates to the Score Object Location in Resources, thus allowing
            // us to save these locations between projects as data.
            ScoreObjectLink scoreObjectLink = newObjectLocationTracker.AddComponent<ScoreObjectLink>();
            scoreObjectLink.ScoringObjectLocation_ = currentScoringObjectData.scoringObject;
            scoreObjectLink.SpawnType_ = currentScoringObjectData.scoringObject.spawnType;
            scoreObjectLink.Index = j;

            newObjectLocationTracker.transform.SetParent(newObject.transform);
            currentScoringObjectData.objectPositions[j] = 
                newObjectLocationTracker.transform;
        }
    }

    void AssignTrackerMaterialAndSetZeroPosition(GameObject newTrackerObject, GameObject trackerImageOfPrefab)
    {
        MeshFilter newMeshFilter = newTrackerObject.AddComponent<MeshFilter>();
        MeshRenderer newMeshRenderer = newTrackerObject.AddComponent<MeshRenderer>();

        if (trackerImageOfPrefab.GetComponent<MeshFilter>() != null)
        {
            newMeshFilter.sharedMesh = trackerImageOfPrefab.GetComponent<MeshFilter>().sharedMesh;
            newMeshRenderer.sharedMaterial = materialIndex.trackerObjectMaterial;
        }
        else
        {
            newMeshFilter.sharedMesh = trackerImageOfPrefab.GetComponentInChildren<MeshFilter>().sharedMesh;
            newMeshRenderer.sharedMaterial = materialIndex.trackerObjectMaterial;
        }

        newTrackerObject.transform.localScale = trackerImageOfPrefab.transform.localScale;
    }
}

