using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ScoringObject))]
public class ScoringObjectCustomEditor : Editor
{
    SerializedProperty objectPrefab, quantityToSpawn, spawnType;
    ScoringObject scoringObject;

    public void OnEnable()
    {
        scoringObject = (ScoringObject)target;
        objectPrefab = serializedObject.FindProperty("objectPrefab");
        quantityToSpawn = serializedObject.FindProperty("quantityToSpawn");
        spawnType = serializedObject.FindProperty("spawnType");
    }

    public override void OnInspectorGUI()
    {
        EditorGUILayout.PropertyField(objectPrefab);
        EditorGUILayout.PropertyField(quantityToSpawn);
        EditorGUILayout.PropertyField(spawnType);

        serializedObject.ApplyModifiedProperties();

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
