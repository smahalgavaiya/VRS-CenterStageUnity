using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ScoringObjectLocation))]
public class ScoringObjectLocationCustomEditor : Editor
{
    SerializedProperty scoreObjectType, quantityToSpawn, spawnType;
    ScoringObjectLocation scoringObject;

    public void OnEnable()
    {
        scoringObject = (ScoringObjectLocation)target;
        scoreObjectType = serializedObject.FindProperty("scoreObjectType");
        quantityToSpawn = serializedObject.FindProperty("quantityToSpawn");
        spawnType = serializedObject.FindProperty("spawnType");
    }

    public override void OnInspectorGUI()
    {
        EditorGUILayout.PropertyField(scoreObjectType);
        EditorGUILayout.PropertyField(quantityToSpawn);
        EditorGUILayout.PropertyField(spawnType);

        serializedObject.ApplyModifiedProperties();

        // If it the locations are at specific points, create the vectors 
        // required to store their locations
        if (scoringObject.spawnType == SpawnType.AtSpecificPoints)
        {
            if (scoringObject.pointPositions == null)
                scoringObject.pointPositions = new List<Vector3>();

            while(scoringObject.pointPositions.Count < scoringObject.quantityToSpawn)
            {
                scoringObject.pointPositions.Add(new Vector3());
            }

            while(scoringObject.pointPositions.Count > scoringObject.quantityToSpawn)
            {
                scoringObject.pointPositions.RemoveAt(scoringObject.pointPositions.Count - 1);
            }

            for (int i = 0; i < scoringObject.pointPositions.Count; i++)
            {
                EditorGUILayout.LabelField("Spawn Point " + (i + 1) + ": " + scoringObject.pointPositions[i].ToString());
            }
        }
        else if (scoringObject.spawnType == SpawnType.RandomOverArea)
        {
            EditorGUILayout.LabelField("Spawn Area Upper Bounds: " + scoringObject.spawnAreaBounds.upperBound.ToString());
            EditorGUILayout.LabelField("Spawn Area Lower Bounds: " + scoringObject.spawnAreaBounds.lowerBound.ToString());
        }

    }
}
