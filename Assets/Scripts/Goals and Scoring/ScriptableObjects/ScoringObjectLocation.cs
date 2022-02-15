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

    public bool isScoringZone; // If this is a zone, is it also a scoring zone?
    public bool showTapeOnField; // Should we show tape on the field where this point or zone exists?
    public Vector3 tapeScale;

    // For specific object locations, or random locations across a series of points
    public List<Vector3> pointPositions;

    // For random locations across a series of points, specifically
    [Range(0,100)]
    public int numberOfPotentialPoints;

    // For random locations in a bounded area
    public Vector3 SpawnScale { get; set; }
    public Vector3 SpawnAreaCenter { get; set; }

    // For stacked at a point
    public Vector3 specificPoint { get; set; }
}


public enum SpawnType
{
    StackedAtPoint, // These spawn at a specific point, either infinitely or finitely
    AtSpecificPoints, // These spawn one each at multiple points
    RandomOverArea, // These spawn randomly over an area
    RandomOverMultiplePoints// These spawn randomly over a series of points
}
