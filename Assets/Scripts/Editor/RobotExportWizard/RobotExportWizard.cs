using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class RobotExportWizard : EditorWindow
{
    public void CreatePopup()
    {
        EditorWindow window = EditorWindow.CreateInstance<RobotExportWizard>();
        EditorStyles.label.wordWrap = true;
        window.Show();
    }

    private void OnGUI()
    {
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Create a new Object Type in the Resources/SpawnableObjects/ScoringObjectTypes folder");
        EditorGUILayout.Space();
        EditorGUILayout.Space();
        EditorGUILayout.Space();

        EditorGUILayout.Space();
        EditorGUILayout.Space();


        if (GUILayout.Button("ok"))
        {
            this.Close();
        }
        
    }


}
