using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ScoreTracker))]
public class ScoreTrackerCustomEditor : Editor
{
    private SerializedProperty currentScore;
    GUIContent label;
    private void OnEnable()
    {
        currentScore = serializedObject.FindProperty("Score");
        label.text = currentScore.intValue.ToString();

    }
    public override void OnInspectorGUI()
    {
        EditorGUILayout.LabelField("Score: " + label.text);

    }
}
