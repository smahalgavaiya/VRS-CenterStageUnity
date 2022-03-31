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
    }

}

