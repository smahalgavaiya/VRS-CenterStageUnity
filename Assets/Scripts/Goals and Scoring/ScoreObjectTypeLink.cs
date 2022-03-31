using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreObjectTypeLink : MonoBehaviour
{
    [SerializeField] ObjectType scoreObjectType;

    public ObjectType ScoreObjectType_
    {
        get
        {
            return scoreObjectType;
        }
        set
        {
            scoreObjectType = value;
        }
    }
}
