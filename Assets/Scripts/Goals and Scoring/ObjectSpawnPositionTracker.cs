using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

[ExecuteInEditMode]
public class ObjectSpawnPositionTracker : MonoBehaviour
{
    public GameObject spawnLocationParent;
    public string resourcesFolder;
    public MaterialIndex materialIndex;

    public ObjectLocation[] ObjectLocations { get; set; }

    private void OnEnable()
    {
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
        ObjectLocations = Resources.LoadAll<ObjectLocation>("SpawnableObjects/" + resourcesFolder);

        //Set this object so we save the data when we save the project
        for (int i = 0; i < ObjectLocations.Length; i++)
        {
            EditorUtility.SetDirty(ObjectLocations[i]);
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
            for (int i = 0; i < ObjectLocations.Length; i++)
            {
                // First create the parent for the empty tracker objects, which should
                // have the same name as the Scoring Object Locations from Resources
                GameObject newObject = 
                    new GameObject(ObjectLocations[i].name + " (" + ObjectLocations[i].spawnType +")");
                newObject.transform.SetParent(spawnLocationParent.transform);

                // Now create the tracker objects themselves, based on which type of
                // Scoring Object Location they are (zone, point, random at multiple points, stacked at point)

                switch (ObjectLocations[i].spawnType)
                {
                    case SpawnType.AtSpecificPoints:
                        CreateTrackersAtSpecificPoints(ObjectLocations[i], newObject);
                        break;
                    case SpawnType.RandomOverArea:
                        CreateZone(ObjectLocations[i], newObject);
                        break;
                    case SpawnType.RandomOverMultiplePoints:
                        CreatePotentialPoints(ObjectLocations[i], newObject);
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

    }

    private void CreatePotentialPoints(ObjectLocation scoringObjectLocation, GameObject newObject)
    {
        // Add a random object spawner to the parent object
        SpawnRandomAcrossMultiplePoints spawnRandomAcrossMultiplePoints = 
            newObject.AddComponent<SpawnRandomAcrossMultiplePoints>();
        spawnRandomAcrossMultiplePoints.objectPrefab = scoringObjectLocation.objectType.objectPrefab;
        spawnRandomAcrossMultiplePoints.NumberToSpawn = scoringObjectLocation.quantityToSpawn;
        Debug.Log(spawnRandomAcrossMultiplePoints.NumberToSpawn);

        // Now create the potential positions
        for (int j = 0; j < scoringObjectLocation.numberOfPotentialPoints; j++)
        {
            GameObject newObjectLocationTracker = new GameObject(scoringObjectLocation.name + " Location Tracker" + (j+1).ToString());

            // Create a "ghost" image of the prefab
            AssignTrackerMaterialAndSetZeroPosition(newObjectLocationTracker, scoringObjectLocation.objectType.objectPrefab);

            // Try to set the position of the tracker object (if it was previously set and saved to the 
            // Scoring Object Location asset)
            if (scoringObjectLocation.pointPositions[j] != Vector3.zero)
                newObjectLocationTracker.transform.position = 
                    scoringObjectLocation.pointPositions[j];
            else 
                newObjectLocationTracker.transform.position += new Vector3((float)j / 10, 0, 0);

            CreateOrDestroyTape(scoringObjectLocation, newObjectLocationTracker);

            // Add the ScoreObjectLink so when we move it around the position data
            // propogates to the Score Object Location in Resources, thus allowing
            // us to save these locations between projects as data.
            ScoreObjectLink scoreObjectLink = newObjectLocationTracker.AddComponent<ScoreObjectLink>();
            scoreObjectLink._ScoringObjectLocation = scoringObjectLocation;
            scoreObjectLink._SpawnType = scoringObjectLocation.spawnType;
            scoreObjectLink.Index = j;

            newObjectLocationTracker.transform.SetParent(newObject.transform);
        }
    }

    private void CreateZone(ObjectLocation scoringObjectLocation, GameObject newObject)
    {
        GameObject newLocationZone = GameObject.CreatePrimitive(PrimitiveType.Cube); 
        newLocationZone.name = scoringObjectLocation.objectType.name + " (" + 
            scoringObjectLocation.spawnType + ")";

        if (scoringObjectLocation.SpawnAreaCenter != Vector3.zero)
        {
            newLocationZone.transform.position = scoringObjectLocation.SpawnAreaCenter;
            newLocationZone.transform.localScale = scoringObjectLocation.SpawnScale;
        } else
            newLocationZone.transform.position = Vector3.zero;

        newLocationZone.GetComponent<Renderer>().sharedMaterial = materialIndex.trackerObjectMaterial;

        // Add the ScoreObjectLink so when we move it around the position data
        // propogates to the Score Object Location in Resources, thus allowing
        // us to save these locations between projects as data.
        ScoreObjectLink scoreObjectLink = newLocationZone.AddComponent<ScoreObjectLink>();
        scoreObjectLink._ScoringObjectLocation = scoringObjectLocation;
        scoreObjectLink._SpawnType = scoringObjectLocation.spawnType;
        scoreObjectLink.Index = 0;

        RandomWithinZoneSpawner randomWithinZoneSpawner = newLocationZone.AddComponent<RandomWithinZoneSpawner>();
        randomWithinZoneSpawner.objectPrefab = scoringObjectLocation.objectType.objectPrefab;
        randomWithinZoneSpawner.numberToSpawn = scoringObjectLocation.quantityToSpawn;

        newLocationZone.transform.parent = newObject.transform;

    }

    void CreateTrackersAtSpecificPoints(ObjectLocation scoringObjectLocation, GameObject newObject)
    {
        for (int j = 0; j < scoringObjectLocation.pointPositions.Count; j++)
        {
            GameObject newObjectLocationTracker = new GameObject(scoringObjectLocation.name + " Location Tracker" + (j+1).ToString());
            SingleObjectSpawner singleObjectSpawner = newObjectLocationTracker.AddComponent<SingleObjectSpawner>();
            singleObjectSpawner.objectPrefab = scoringObjectLocation.objectType.objectPrefab;

            // Create a "ghost" image of the prefab
            AssignTrackerMaterialAndSetZeroPosition(newObjectLocationTracker, scoringObjectLocation.objectType.objectPrefab);

            // Try to set the position of the tracker object (if it was previously set and saved to the 
            // Scoring Object Location asset)
            if (scoringObjectLocation.pointPositions[j] != Vector3.zero)
                newObjectLocationTracker.transform.position = 
                    scoringObjectLocation.pointPositions[j];
            else 
                newObjectLocationTracker.transform.position += new Vector3((float)j / 10, 0, 0);

            CreateOrDestroyTape(scoringObjectLocation, newObjectLocationTracker);

            // Add the ScoreObjectLink so when we move it around the position data
            // propogates to the Score Object Location in Resources, thus allowing
            // us to save these locations between projects as data.
            ScoreObjectLink scoreObjectLink = newObjectLocationTracker.AddComponent<ScoreObjectLink>();
            scoreObjectLink._ScoringObjectLocation = scoringObjectLocation;
            scoreObjectLink._SpawnType = scoringObjectLocation.spawnType;
            scoreObjectLink.Index = j;

            newObjectLocationTracker.transform.SetParent(newObject.transform);
        }
    }
    void CreateOrDestroyTape(ObjectLocation scoringObjectLocation, GameObject trackerObject)
    {
        if (scoringObjectLocation.showTapeOnField)
        {
            SingleTapeMaker singleTapeMaker = trackerObject.AddComponent<SingleTapeMaker>();
            singleTapeMaker.tapeScale = scoringObjectLocation.tapeScale;
        }
        else
        {
            if (trackerObject.GetComponent<SingleTapeMaker>() != null)
                DestroyImmediate(trackerObject.GetComponent<SingleTapeMaker>());
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

