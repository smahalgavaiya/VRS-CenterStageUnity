using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(GoalZoneScorer))]
public class GoalZoneScorerCustomEditor : Editor
{
    GoalZoneScorer goalZoneScorer;
    SerializedProperty roundScores;
    private void OnEnable()
    {
        goalZoneScorer = (GoalZoneScorer)target;
        roundScores = serializedObject.FindProperty("roundScores");
    }

    public override void OnInspectorGUI()
    {


        EditorGUILayout.LabelField("Enter the point value per round for this object");
        EditorGUILayout.Space();
        try
        {
            for (int i = 0; i < roundScores.arraySize; i++) {
                EditorGUILayout.PropertyField(roundScores.GetArrayElementAtIndex(i), 
                    new GUIContent(goalZoneScorer.roundIndex.rounds[i].roundName));
            }
        }
        catch
        {
            return;
        }
    }
}
