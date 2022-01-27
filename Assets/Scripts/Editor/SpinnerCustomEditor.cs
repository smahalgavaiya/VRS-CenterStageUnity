using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Spinner))]
public class SpinnerCustomEditor : Editor
{
    Spinner spinner;

    private void OnEnable()
    {
        spinner = (Spinner)target;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        EditorGUILayout.Space();

        GUIStyle guiStyle = EditorStyles.label;
        guiStyle.wordWrap = true;

        GUILayout.Label("The Spinner component should be placed on physics objects that are close to Root in the scene hierarchy. It should NOT be placed deep inside model hierarchies. If you need to connect the spinner to a model to make a part move, create a Driver component and connect it to part of a model.",
            guiStyle);

        GUILayout.Space(20);
            

        if (GUILayout.Button("Create Driver"))
        {
            spinner.CreateSpinDriver();
        }
    }
}
