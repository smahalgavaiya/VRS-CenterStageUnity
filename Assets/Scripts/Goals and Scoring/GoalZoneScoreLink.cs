using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GoalZoneScoreLink : MonoBehaviour
{
    ScoreTrackerIndex scoreTrackerIndex;
    ScoringGuide scoringGuide;
    RoundIndex roundIndex;

    // These will run to verify whether something can score based on custom checkers, e.g. object orientation
    List<ICustomGoalChecker> customGoalCheckers;
    List<ICustomGoalEvents> customGoalEvents;

    //We may want an optional bool value that determines when this triggers
    [SerializeField]
    [Tooltip("This determines whether to use the Optional Bool value-- sometimes you want the " +
        "goal zone to trigger only under certain circumstances, which can be set from outside" +
        "this class.")]
    bool useOptionalBool;
    [Tooltip("This determines whether to use the Optional Bool value-- sometimes you want the " +
        "goal zone to trigger only under certain circumstances, which can be set from outside" +
        "this class.")]
    [SerializeField] bool useCustomGoalCheckers, useCustomGoalEvents;
    [SerializeField]
    [Tooltip("If you want to evaluate a GlobalBool to determine whether this triggers. " +
        "If you aren't using a GlobalBool, you can still speak directly to this class using the OptionalBoolValue property.")]
    GlobalBool globalBool;
    bool optionalBoolValue;
    public bool OptionalBoolValue { get { if (globalBool != null) return globalBool.boolValue; else return optionalBoolValue; } set { optionalBoolValue = value; } }

    TeamColor scoreZoneColor;

    public TeamColor LastObjectTeamColor { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        scoreTrackerIndex = GetComponent<GoalZoneBaseData>().scoreTrackerIndex;
        scoringGuide = GetComponent<GoalZoneBaseData>().scoringGuide;
        scoreZoneColor = GetComponent<GoalZoneBaseData>().scoreZoneColor;

        if (useCustomGoalCheckers)
        {
            customGoalCheckers = new List<ICustomGoalChecker>();
            foreach (ICustomGoalChecker customGoalChecker in GetComponents<ICustomGoalChecker>())
            {
                customGoalCheckers.Add(customGoalChecker);
            }
        }

        if (useCustomGoalEvents)
        {
            customGoalEvents = new List<ICustomGoalEvents>();
            foreach (ICustomGoalEvents customGoalEvent in GetComponents<ICustomGoalEvents>())
            {
                customGoalEvents.Add(customGoalEvent);
            }
        }
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

    private void RunCustomChecks(Collider other)
    {
        foreach (ICustomGoalChecker customGoalChecker in customGoalCheckers)
        {
            customGoalChecker.DoCustomCheck(other.gameObject);
        }
    }
    // Check if the object's tag is listed in the array of ScoreObjectTypes, 
    // and if it is, get its index in the array
    void CheckForObjectInScoreTypes(Collider other, int scoreDirection)
    {
        int scoreObjectTypeIndex;

        ObjectType collidedObjectType = null;

        ScoreObjectTypeLink scoreObjectTypeLink = null;

        if (other.GetComponent<ScoreObjectTypeLink>() != null)
            scoreObjectTypeLink = other.GetComponent<ScoreObjectTypeLink>();
        else if (other.GetComponentInChildren<ScoreObjectTypeLink>() != null)
            scoreObjectTypeLink = other.GetComponentInChildren<ScoreObjectTypeLink>();
        else if (other.GetComponentInParent<ScoreObjectTypeLink>() != null)
            scoreObjectTypeLink = other.GetComponentInParent<ScoreObjectTypeLink>();


        if (scoreObjectTypeLink == null || scoreObjectTypeLink.ScoreObjectType_ == null)
        {
            Debug.Log(this.gameObject);
            Debug.Log(other.gameObject);
            Debug.Log("Your prefab is either missing the ScoreObjectTypeLink component or the prefab's ScoreObjectTypeLink is missing a reference to the Score Object Type");
            return;
        }

        else collidedObjectType = scoreObjectTypeLink.ScoreObjectType_;

        foreach (ObjectType objectType in scoringGuide.scoreObjectTypes)
        {
            if (collidedObjectType == objectType)
            {
                scoreObjectTypeIndex = Array.FindIndex(scoringGuide.scoreObjectTypes, w => w == collidedObjectType);

                if (useCustomGoalCheckers)
                    RunCustomChecks(other);

                if (useOptionalBool && !optionalBoolValue)
                    return;
                TeamColor lastTeamTouched = scoreObjectTypeLink.LastTouchedTeamColor;
                ChangeScore(scoreObjectTypeIndex, scoringGuide, scoreDirection, lastTeamTouched);

                LastObjectTeamColor = lastTeamTouched;

                if (useCustomGoalEvents)
                {
                    switch(scoreDirection)
                    {
                        case 1: 
                            RunCustomOnEvents();
                            break;
                        case -1: 
                            RunCustomOffEvents();
                            break;
                    }
                }
            }
        }
    }

    private void RunCustomOnEvents()
    {
        foreach (ICustomGoalEvents customGoalEvent in customGoalEvents)
        {
            customGoalEvent.DoCustomOnEvent(this);
        }
    }
    private void RunCustomOffEvents()
    {
        foreach (ICustomGoalEvents customGoalEvent in customGoalEvents)
        {
            customGoalEvent.DoCustomOffEvent(this);
        }
    }

    public void ChangeScore(int scoreTypeIndex, ScoringGuide scoringGuideLocal, int scoreDirection, TeamColor lastTeamTouched)
    {
        int currentRound = scoringGuideLocal.roundIndex.currentRound;
        int scoreAmount = scoringGuideLocal.scoresPerRoundPerType[scoreTypeIndex].scoresPerRound[currentRound];

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
