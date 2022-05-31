using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class ScoreObjectLink : MonoBehaviour
{
    public ObjectLocation scoringObjectLocation;
    public int indexOfTracker;
    public SpawnType spawnType;

    // Update is called once per frame
    void Update()
    {
        if (spawnType == SpawnType.AtSpecificPoints || spawnType == SpawnType.RandomOverMultiplePoints)
        {
            scoringObjectLocation.pointPositions[indexOfTracker] = transform.position;
        }
        else if (spawnType == SpawnType.RandomOverArea)
        {
            scoringObjectLocation.SpawnAreaCenter = transform.position;
            scoringObjectLocation.SpawnScale = transform.localScale;
        }
        else if (spawnType == SpawnType.StackedAtPoint)
        {
            scoringObjectLocation.specificPoint = transform.position;
        }
    }
}
