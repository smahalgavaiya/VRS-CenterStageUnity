using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ObjectType))]
public class ObjectTypeCustomEditor : Editor
{
    ObjectType objectType;
    private void OnEnable()
    {
        objectType = (ObjectType)target;
    }

    public override void OnInspectorGUI()
    {
        EditorStyles.label.wordWrap = true;
        base.OnInspectorGUI();
    }

}

