using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ScoreObjectType))]
public class ScoreObjectTypeCustomEditor : Editor
{
    ScoreObjectType scoreObjectType;
    private void OnEnable()
    {
        scoreObjectType = (ScoreObjectType)target;
    }

    public override void OnInspectorGUI()
    {
        EditorStyles.label.wordWrap = true;
        base.OnInspectorGUI();

        EditorGUILayout.Space();
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("You can automatically set the tags of the prefab here. If the tag doesn't exist, it will be created.");
        EditorGUILayout.Space();
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Tags must match the Score Object Type name for the Goal Zones to properly detect the object");
        EditorGUILayout.Space();
        EditorGUILayout.Space();
        if (GUILayout.Button("Set tags for object prefab"))
        {
            try
            {
                TagManager.CreateTag(scoreObjectType.name);
                scoreObjectType.objectPrefab.tag = scoreObjectType.name;
                foreach (Transform transform in scoreObjectType.objectPrefab.GetComponentsInChildren<Transform>())
                {
                    transform.tag = scoreObjectType.name;
                }
            } catch
            {
                EditorUtility.DisplayDialog("No prefab assigned", "You need to set an object prefab first", "ok");
            }

        }
    }

}

public static class TagManager
{
    public static void CreateTag(string tag) {
        var asset = AssetDatabase.LoadMainAssetAtPath("ProjectSettings/TagManager.asset");
        if (asset != null) { // sanity checking
            var so = new SerializedObject(asset);
            var tags = so.FindProperty("tags");

            var numTags = tags.arraySize;
            // do not create duplicates
            for (int i = 0; i < numTags; i++) {
            var existingTag = tags.GetArrayElementAtIndex(i);
            if (existingTag.stringValue == tag) return;
        }

        tags.InsertArrayElementAtIndex(numTags);
        tags.GetArrayElementAtIndex(numTags).stringValue = tag;
        so.ApplyModifiedProperties();
        so.Update();
        }
    }
}
