using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ScoringManager : MonoBehaviour
{
    [SerializeField] ScoreTracker blueScoreTracker;
    [SerializeField] ScoreTracker redScoreTracker;

    private static Dictionary<Team, ScoreTracker> scores = new Dictionary<Team, ScoreTracker>();
    // Start is called before the first frame update
    void Start()
    {
        scores.Clear();
        scores.Add(Team.blue, blueScoreTracker);
        scores.Add(Team.red, redScoreTracker);
    }

    public static void ScoreEvent(Team team, int amount, string reason, GameObject reportingObj)
    {
        //pass scoring type data so we can assign points based on current round
        Debug.Log($"{team} Score: {amount} for {reason} ({reportingObj})");
        scores[team].AddOrSubtractScore(amount);
    }
}
