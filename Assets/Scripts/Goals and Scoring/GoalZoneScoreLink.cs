using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GoalZoneScoreLink : MonoBehaviour
{
    ScoreTrackerIndex scoreTrackerIndex;
    ScoringGuide scoringGuide;
    RoundIndex roundIndex;

    //We may want an optional bool value that determines when this triggers
    [SerializeField]
    [Tooltip("This determines whether to use the Optional Bool value-- sometimes you want the " +
        "goal zone to trigger only under certain circumstances, which can be set from outside" +
        "this class.")]
    bool useOptionalBool;
    [SerializeField]
    [Tooltip("If you want to evaluate a GlobalBool to determine whether this triggers. " +
        "If you aren't using a GlobalBool, you can still speak directly to this class using the OptionalBoolValue property.")]
    GlobalBool globalBool;
    bool optionalBoolValue;
    public bool OptionalBoolValue { get { if (globalBool != null) return globalBool.boolValue; else return optionalBoolValue; } set { optionalBoolValue = value; } }

    TeamColor scoreZoneColor;

    // Start is called before the first frame update
    void Start()
    {
        scoreTrackerIndex = GetComponent<GoalZoneBaseData>().scoreTrackerIndex;
        scoringGuide = GetComponent<GoalZoneBaseData>().scoringGuide;
        scoreZoneColor = GetComponent<GoalZoneBaseData>().scoreZoneColor;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        CheckForObjectInScoreTypes(other, 1);
    }
    private void OnTriggerExit(Collider other)
    {
        CheckForObjectInScoreTypes(other, -1);
    }

    // Check if the object's tag is listed in the array of ScoreObjectTypes, 
    // and if it is, get its index in the array
    void CheckForObjectInScoreTypes(Collider other, int scoreDirection)
    {
        int scoreObjectTypeIndex;

        ObjectType collidedObjectType = null;
        if (other.GetComponent<ScoreObjectTypeLink>() == null || other.GetComponent<ScoreObjectTypeLink>().ScoreObjectType_ == null)
        {
            Debug.Log(this.gameObject);
            Debug.Log("Your prefab is either missing the ScoreObjectTypeLink component or the prefab's ScoreObjectTypeLink is missing a reference to the Score Object Type");
            return;
        }
        else collidedObjectType = other.GetComponent<ScoreObjectTypeLink>().ScoreObjectType_;

        foreach (ObjectType objectType in scoringGuide.scoreObjectTypes)
        {

            if (collidedObjectType == objectType)
            {
                scoreObjectTypeIndex = Array.FindIndex(scoringGuide.scoreObjectTypes, w => w == collidedObjectType);
                if (useOptionalBool && !optionalBoolValue)
                    return;
                TeamColor lastTeamTouched = other.GetComponent<ScoreObjectTypeLink>().LastTouchedTeamColor;
                ChangeScore(scoreObjectTypeIndex, scoreDirection, lastTeamTouched);
            }
        }
    }


    void ChangeScore(int scoreTypeIndex, int scoreDirection, TeamColor lastTeamTouched)
    {
        int currentRound = scoringGuide.roundIndex.currentRound;
        int scoreAmount = scoringGuide.scoresPerRoundPerType[scoreTypeIndex].scoresPerRound[currentRound];
        
        // Based on the ScoreZoneColor, increase or decrease that team's score
        // or, if it is Either, check the color info on the object
        switch(scoreZoneColor)
        {
            case TeamColor.Blue:
                scoreTrackerIndex.blueScoreTracker.AddOrSubtractScore(scoreAmount * scoreDirection);
                break;
            case TeamColor.Red:
                scoreTrackerIndex.redScoreTracker.AddOrSubtractScore(scoreAmount * scoreDirection);
                break;
            case TeamColor.Either:
                if (lastTeamTouched == TeamColor.Red)
                    scoreTrackerIndex.redScoreTracker.AddOrSubtractScore(scoreAmount * scoreDirection);
                else
                    scoreTrackerIndex.blueScoreTracker.AddOrSubtractScore(scoreAmount * scoreDirection);
                break;
        }
    }
}
