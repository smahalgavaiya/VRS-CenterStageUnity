using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ScoringObjectSpawnPositionTracker))]
public class ObjectSpawnLocationTrackerCustomEditor : Editor
{
    ScoringObjectSpawnPositionTracker objectSpawnLocationTracker;

    private void OnEnable()
    {
        objectSpawnLocationTracker = (ScoringObjectSpawnPositionTracker)target;

    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        EditorGUILayout.Space();

        if (GUILayout.Button("Create Score Object Location Empty Objects"))
        {
            objectSpawnLocationTracker.CreateObjectSpawnLocationEmptyObjects();
        }

    }
}
