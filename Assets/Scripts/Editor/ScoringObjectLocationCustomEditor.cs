using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ScoringObjectLocation))]
public class ScoringObjectLocationCustomEditor : Editor
{
    SerializedProperty scoreObjectType, quantityToSpawn, spawnType, 
        showTapeOnField, isScoringZone, numberOfPotentialPoints;
    ScoringObjectLocation scoringObject;

    public void OnEnable()
    {
        scoringObject = (ScoringObjectLocation)target;
        scoreObjectType = serializedObject.FindProperty("scoreObjectType");
        quantityToSpawn = serializedObject.FindProperty("quantityToSpawn");
        spawnType = serializedObject.FindProperty("spawnType");
        showTapeOnField = serializedObject.FindProperty("showTapeOnField");
        isScoringZone = serializedObject.FindProperty("isScoringZone");
        numberOfPotentialPoints = serializedObject.FindProperty("numberOfPotentialPoints");
    }

    public override void OnInspectorGUI()
    {
        EditorGUIUtility.labelWidth = 160;

        EditorGUILayout.PropertyField(scoreObjectType);
        EditorGUILayout.PropertyField(spawnType);
        EditorGUILayout.PropertyField(quantityToSpawn);
        EditorGUILayout.PropertyField(showTapeOnField);

        serializedObject.ApplyModifiedProperties();

        // If it the locations are at specific points, or random across a series of points, create the vectors 
        // required to store their locations
        if (scoringObject.spawnType == SpawnType.AtSpecificPoints ||
            scoringObject.spawnType == SpawnType.RandomOverMultiplePoints)
        {
            int numberToUseForListCount = 0; // We use a different number depending on the SpawnType

            switch (scoringObject.spawnType)
            {
                case SpawnType.AtSpecificPoints:
                    numberToUseForListCount = scoringObject.quantityToSpawn;
                    break;
                case SpawnType.RandomOverMultiplePoints:
                    EditorGUILayout.PropertyField(numberOfPotentialPoints);
                    serializedObject.ApplyModifiedProperties();
                    numberToUseForListCount = scoringObject.numberOfPotentialPoints;
                    break;
            }

            if (scoringObject.pointPositions == null)
                scoringObject.pointPositions = new List<Vector3>();

            while(scoringObject.pointPositions.Count < numberToUseForListCount)
            {
                scoringObject.pointPositions.Add(new Vector3());
            }

            while(scoringObject.pointPositions.Count > numberToUseForListCount)
            {
                scoringObject.pointPositions.RemoveAt(scoringObject.pointPositions.Count - 1);
            }

            string spawnTypeSpecificString = "";

            for (int i = 0; i < scoringObject.pointPositions.Count; i++)
            {
                switch (scoringObject.spawnType)
                {
                    case SpawnType.AtSpecificPoints:
                        spawnTypeSpecificString = "Spawn Point ";
                        break;
                    case SpawnType.RandomOverMultiplePoints:
                        spawnTypeSpecificString = "Possible Spawn Point ";
                        break;
                }
                EditorGUILayout.LabelField(spawnTypeSpecificString + (i + 1) + ": " + scoringObject.pointPositions[i].ToString());
            }
        }

        else if (scoringObject.spawnType == SpawnType.RandomOverArea)
        {
            EditorGUILayout.PropertyField(isScoringZone);

            serializedObject.ApplyModifiedProperties();

            EditorGUILayout.LabelField("Spawn Area Upper Bounds: " + scoringObject.spawnAreaBounds.upperBound.ToString());
            EditorGUILayout.LabelField("Spawn Area Lower Bounds: " + scoringObject.spawnAreaBounds.lowerBound.ToString());
            EditorGUILayout.LabelField("Spawn Area Center: " + scoringObject.spawnAreaCenter.ToString());
        }
        else if (scoringObject.spawnType == SpawnType.StackedAtPoint)
        {
            EditorGUILayout.LabelField("Point for Stack: " + scoringObject.specificPoint.ToString());
        }

    }
}
