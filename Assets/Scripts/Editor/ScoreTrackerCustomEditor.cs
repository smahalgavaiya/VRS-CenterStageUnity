using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ScoreTracker))]
public class ScoreTrackerCustomEditor : Editor
{
    ScoreTracker scoreTracker;

    private void OnEnable()
    {
        scoreTracker = (ScoreTracker)target;
    }
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        EditorGUILayout.LabelField("Score: " + scoreTracker.Score);
    }
}
