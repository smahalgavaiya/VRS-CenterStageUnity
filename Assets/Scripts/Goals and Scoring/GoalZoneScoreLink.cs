using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GoalZoneScoreLink : MonoBehaviour
{
    ScoreTrackerIndex scoreTrackerIndex;
    ScoringGuide scoringGuide;
    RoundIndex roundIndex;

    ScoreZoneColor scoreZoneColor;

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

        foreach (ScoreObjectType objectType in scoringGuide.scoreObjectTypes)
        {
            if (other.tag == objectType.name.ToString())
            {
                scoreObjectTypeIndex = Array.FindIndex(scoringGuide.scoreObjectTypes, w => w.name == other.tag);
                ChangeScore(scoreObjectTypeIndex, scoreDirection);
            }
        }
    }
    void ChangeScore(int scoreTypeIndex, int scoreDirection)
    {
        int currentRound = scoringGuide.roundIndex.currentRound;
        int scoreAmount = scoringGuide.scoresPerRoundPerType[scoreTypeIndex].scoresPerRound[currentRound];
        
        // Based on the ScoreZoneColor, increase or decrease that team's score
        // or, if it is Either, check the color info on the object
        switch(scoreZoneColor)
        {
            case ScoreZoneColor.Blue:
                scoreTrackerIndex.blueScoreTracker.AddOrSubtractScore(scoreAmount * scoreDirection);
                break;
            case ScoreZoneColor.Red:
                scoreTrackerIndex.redScoreTracker.AddOrSubtractScore(scoreAmount * scoreDirection);
                break;
            case ScoreZoneColor.Either:
                break;
        }
    }
}
