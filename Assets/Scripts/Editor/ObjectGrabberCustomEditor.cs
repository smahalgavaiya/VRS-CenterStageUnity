using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ObjectGrabber))]
public class ObjectGrabberCustomEditor : Editor
{
    SerializedProperty customHoldingLocation;
    SerializedProperty hasCustomHoldingLocation;
    SerializedProperty loadObjectLauncher;
    SerializedProperty objectLauncher;
    SerializedProperty objectGrabberDrive;
    private void OnEnable()
    {
        hasCustomHoldingLocation = serializedObject.FindProperty("hasCustomHoldingLocation");
        customHoldingLocation = serializedObject.FindProperty("customHoldingLocation");
        loadObjectLauncher = serializedObject.FindProperty("loadObjectLauncher");
        objectLauncher = serializedObject.FindProperty("objectLauncher");
        objectGrabberDrive = serializedObject.FindProperty("objectGrabberDrive");
    }

    public override void OnInspectorGUI()
    {
        EditorGUIUtility.labelWidth = 200;
        EditorGUILayout.PropertyField(hasCustomHoldingLocation);
        EditorGUILayout.PropertyField(loadObjectLauncher);
        EditorGUILayout.PropertyField(objectGrabberDrive);


        if (hasCustomHoldingLocation.boolValue)
        {
            EditorGUILayout.PropertyField(customHoldingLocation);
        }

        if (loadObjectLauncher.boolValue)
        {
            EditorGUILayout.PropertyField(objectLauncher);
        }

        serializedObject.ApplyModifiedProperties();
    }
}
