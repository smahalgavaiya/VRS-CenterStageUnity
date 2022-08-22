using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddPointsForNewRound : MonoBehaviour, ICustomGoalEvents
{
    int numberOfBlueObjectsToCheck, numberOfRedObjectsToCheck;

    [SerializeField] RoundIndex roundIndex;
    ScoreTrackerIndex scoreIndex;
    GoalZoneBaseData goalZoneBaseData;
    ScoringGuide scoringGuide;

    bool itemsAdded = false;

    // Start is called before the first frame update
    void Start()
    {
        goalZoneBaseData = GetComponent<GoalZoneBaseData>();
        scoringGuide = goalZoneBaseData.scoringGuide;
        scoreIndex = goalZoneBaseData.scoreTrackerIndex;
    }

    // Update is called once per frame
    void Update()
    {
        if (roundIndex.currentRound == 1 && !itemsAdded)
        {
            scoreIndex.blueScoreTracker.AddOrSubtractScore(numberOfBlueObjectsToCheck * scoringGuide.scoresPerRoundPerType[1].scoresPerRound[1]);
            scoreIndex.redScoreTracker.AddOrSubtractScore(numberOfRedObjectsToCheck * scoringGuide.scoresPerRoundPerType[1].scoresPerRound[1]);
            itemsAdded = true;
        }
    }
    public void DoCustomOffEvent(Object objectToPass)
    {
        if (roundIndex.currentRound != 0)
            return;

        GoalZoneScoreLink goalZoneScoreLink = (GoalZoneScoreLink)objectToPass;
        if (goalZoneScoreLink.LastObjectTeamColor == TeamColor.Blue)
        {
            numberOfBlueObjectsToCheck--;
        } else numberOfRedObjectsToCheck--;
    }

    public void DoCustomOnEvent(Object objectToPass)
    {
        if (roundIndex.currentRound != 0)
            return;

        GoalZoneScoreLink goalZoneScoreLink = (GoalZoneScoreLink)objectToPass;
        if (goalZoneScoreLink.LastObjectTeamColor == TeamColor.Blue)
        {
            numberOfBlueObjectsToCheck++;
        } else numberOfRedObjectsToCheck++;
    }
}
