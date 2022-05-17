using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreObjectTypeLink : MonoBehaviour
{
    [SerializeField] ObjectType scoreObjectType;

    [SerializeField] ScoreZoneColor lastTouchedTeamColor = ScoreZoneColor.Either;
    public ScoreZoneColor LastTouchedTeamColor { get { return lastTouchedTeamColor; } }

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
