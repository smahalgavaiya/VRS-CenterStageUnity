using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[ExecuteInEditMode]
public class ScoringObjectSpawnPositionTracker : MonoBehaviour
{
    public GameObject spawnLocationParent;
    public string resourcesFolder;
    ScoringObjectSpawnManager scoringObjectSpawnManager;

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
            scoringObjectSpawnManager.scoringObjects[i].objectPositions = new Transform[scoringObjectLocations[i].quantityToSpawn];
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
            // First create the parents for the empty tracker objects, which should
            // have the same name as the Scoring Object Locations from Resources
            for (int i = 0; i < scoringObjectSpawnManager.scoringObjects.Length; i++)
            {
                GameObject newObject = 
                    new GameObject(scoringObjectSpawnManager.scoringObjects[i].objectName);
                newObject.transform.SetParent(spawnLocationParent.transform);

                // Now create the tracker objects themselves, based on the quantity defined
                // in the Resource Score Object Location quantityToSpawn, which propogates
                // to the objectPositions of the array above. 
                // 
                // We use the length of that array instead of quantityToSpawn just to 
                // be careful that we don't exceed the size of the array.

                for (int j = 0; j < scoringObjectSpawnManager.scoringObjects[i].objectPositions.Length; j++)
                {
                    GameObject newObjectLocationTracker = new GameObject(scoringObjectSpawnManager.scoringObjects[i].objectName + " Location Tracker" + (j+1).ToString());
                    //newObjectLocationTracker.AddComponent<MeshFilter>();
                    //newObjectLocationTracker.AddComponent<MeshRenderer>();

                    //// Set the new tracker object mesh to the same as the scoring object type
                    //try
                    //{
                    //    newObjectLocationTracker.GetComponent<MeshFilter>().mesh =
                    //        scoringObjectSpawnManager.scoringObjects[i].scoringObject.scoreObjectType.objectPrefab.GetComponent<MeshFilter>().sharedMesh; 
                    //}
                    //catch
                    //{
                    //    newObjectLocationTracker.GetComponent<MeshFilter>().mesh =
                    //        scoringObjectSpawnManager.scoringObjects[i].scoringObject.scoreObjectType.objectPrefab.GetComponentInChildren<MeshFilter>().sharedMesh; 
                    //}

                    // Try to set the position of the tracker object
                    if (scoringObjectSpawnManager.scoringObjects[i].scoringObject.pointPositions[j] != null)
                        newObjectLocationTracker.transform.position = 
                            scoringObjectSpawnManager.scoringObjects[i].scoringObject.pointPositions[j];

                    // Add the ScoreObjectLink so when we move it around the position data
                    // propogates to the Score Object Location in Resources, thus allowing
                    // us to save these locations between projects as data.
                    ScoreObjectLink scoreObjectLink = newObjectLocationTracker.AddComponent<ScoreObjectLink>();
                    scoreObjectLink.ScoringObjectLocation_ = scoringObjectSpawnManager.scoringObjects[i].scoringObject;
                    scoreObjectLink.SpawnType_ = scoringObjectSpawnManager.scoringObjects[i].scoringObject.spawnType;
                    scoreObjectLink.Index = j;

                    newObjectLocationTracker.transform.SetParent(newObject.transform);
                    scoringObjectSpawnManager.scoringObjects[i].objectPositions[j] = 
                        newObjectLocationTracker.transform;
                }
            }
        }

    }
}

