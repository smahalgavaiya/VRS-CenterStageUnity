using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ScoreTarget))]
public class ScoreTargetCustomEditor : Editor
{
    ScoreTarget scoreTarget;
    SerializedProperty roundScores;
    private void OnEnable()
    {
        scoreTarget = (ScoreTarget)target;
        roundScores = serializedObject.FindProperty("roundScores");
    }

    public override void OnInspectorGUI()
    {

        if (scoreTarget.roundScores.Count < scoreTarget.roundIndex.rounds.Count)
        {
            scoreTarget.roundScores.Add(0);
        }
        else if (scoreTarget.roundScores.Count > scoreTarget.roundIndex.rounds.Count)
        {
            scoreTarget.roundScores.RemoveAt(scoreTarget.roundScores.Count - 1);
        }


        EditorGUILayout.LabelField("Enter the point value per round for this object");
        EditorGUILayout.Space();
        try
        {
            for (int i = 0; i < roundScores.arraySize; i++) {
                EditorGUILayout.PropertyField(roundScores.GetArrayElementAtIndex(i), 
                    new GUIContent(scoreTarget.roundIndex.rounds[i].roundName));
            }
        }
        catch
        {
            return;
        }
    }
}
