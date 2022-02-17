using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ScoringObjectSpawnPositionTracker))]
public class ObjectSpawnLocationTrackerCustomEditor : Editor
{
    ScoringObjectSpawnPositionTracker scoringObjectSpawnPositionTracker;

    private void OnEnable()
    {
        scoringObjectSpawnPositionTracker = (ScoringObjectSpawnPositionTracker)target;
    }

    public override void OnInspectorGUI()
    {
        EditorGUIUtility.labelWidth = 160;
        base.OnInspectorGUI();
        if (GUILayout.Button("Get or create path"))
        {
            string relativePath = "/Resources/SpawnableObjects/"; 
            string folderPath = EditorUtility.OpenFolderPanel("Get or create path", "Assets" + relativePath, "");

            scoringObjectSpawnPositionTracker.resourcesFolder = folderPath.Substring(Application.dataPath.Length + relativePath.Length);
            serializedObject.ApplyModifiedProperties();
            serializedObject.Update();
        }
        EditorStyles.label.wordWrap = true;
        EditorGUILayout.Space();
        EditorGUILayout.Space();

        EditorGUILayout.LabelField("This button will create empty tracker objects based on the scoring zones in the Resources folder specified above.");
        if (GUILayout.Button("Create score object location empty tracker objects"))
        {
            scoringObjectSpawnPositionTracker.CreateObjectSpawnLocationEmptyObjects();
        }

        EditorGUILayout.Space();
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("By default, when you move the tracker objects around, their locations are saved to the Scoring Zone objects in the Resources folder specified above, so that you can easily save location data for different configurations. If you want to reset those values, hit this button.");

        if (GUILayout.Button("Reset all locations to zero position."))
        {
            if (scoringObjectSpawnPositionTracker.spawnLocationParent.transform.childCount < 1)
            {
                if (EditorUtility.DisplayDialog("No tracker objects created", "No tracker objects created, would you like to create them now?", "Yes, create them", "No"))
                {
                    scoringObjectSpawnPositionTracker.CreateObjectSpawnLocationEmptyObjects();
                } else
                    return;
            }
            if (EditorUtility.DisplayDialog("Are you sure?", "This will reset ALL tracker locations to zero. If you only need to do a few, it's better to do them manually.", "Yes, reset them all.", "No thanks."))
            {
                foreach(Transform trackerObjectTransform in scoringObjectSpawnPositionTracker.spawnLocationParent.transform.GetComponentsInChildren<Transform>())
                {
                    trackerObjectTransform.position = Vector3.zero;
                }
            }
        }

    }
}
