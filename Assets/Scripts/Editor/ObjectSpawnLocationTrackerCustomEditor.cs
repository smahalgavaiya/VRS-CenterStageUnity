using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ObjectSpawnLocationTracker))]
public class ObjectSpawnLocationTrackerCustomEditor : Editor
{
    ObjectSpawnLocationTracker objectSpawnLocationTracker;

    private void OnEnable()
    {
        objectSpawnLocationTracker = (ObjectSpawnLocationTracker)target;

    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        EditorGUILayout.Space();

        if (GUILayout.Button("Create Spawn Location Empty Objects"))
        {
            objectSpawnLocationTracker.CreateObjectSpawnLocationEmptyObjects();
        }

    }
}
