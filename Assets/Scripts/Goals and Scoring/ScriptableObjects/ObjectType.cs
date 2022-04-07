using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Dynamic Objects/Object Type")]
public class ObjectType : ScriptableObject
{
    public GameObject objectPrefab;
    public bool isScoringObject;

    private void OnValidate()
    {
        if (isScoringObject)
            SetObjectPrefabObjectType();
    }

    public void SetObjectPrefabObjectType()
    {
        if (objectPrefab != null)
        {
            if (!objectPrefab.GetComponent<ScoreObjectTypeLink>())
            {
                objectPrefab.AddComponent<ScoreObjectTypeLink>();
                objectPrefab.GetComponent<ScoreObjectTypeLink>().ScoreObjectType_ = this;
            } else if (objectPrefab.GetComponent<ScoreObjectTypeLink>())
            {
                objectPrefab.GetComponent<ScoreObjectTypeLink>().ScoreObjectType_ = this;
            }
        }
    }

}
