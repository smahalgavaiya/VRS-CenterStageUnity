using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreObjectTypeLink : MonoBehaviour
{
    [SerializeField] ScoreObjectType scoreObjectType;

    public ScoreObjectType ScoreObjectType_
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
