using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scoring/Score Object Type")]
public class ScoreObjectType : ScriptableObject
{
    public GameObject objectPrefab;

    private void OnValidate()
    {
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
