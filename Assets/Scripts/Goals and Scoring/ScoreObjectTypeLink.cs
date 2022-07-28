using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreObjectTypeLink : MonoBehaviour
{
    [SerializeField] ObjectType scoreObjectType;

    [SerializeField] TeamColor lastTouchedTeamColor = TeamColor.Either;
    public TeamColor LastTouchedTeamColor { get { return lastTouchedTeamColor; } }

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
