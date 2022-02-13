using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scoring/Scoring Object Location")]
public class ScoringObjectLocation: ScriptableObject
{
    public ScoreObjectType scoreObjectType;
    [Range(0,100)]
    public int quantityToSpawn;
    public SpawnType spawnType;

    public List<Vector3> pointPositions { get; set; }

    public SpawnAreaBounds spawnAreaBounds { get; set; }
}

[System.Serializable]
public class SpawnAreaBounds
{
    public Vector3 lowerBound, upperBound;
}

public enum SpawnType
{
    StackedAtPoint, // These spawn at a specific point, either infinitely or finitely
    AtSpecificPoints, // These spawn one each at multiple points
    RandomOverArea, // These spawn randomly over an area
    RandomOverMultiplePoints// These spawn randomly over a series of points
}
