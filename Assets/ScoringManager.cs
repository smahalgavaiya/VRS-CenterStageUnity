using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ScoreEntry
{
    public string reason;
    public Team team;
    public string details;
    public int amount;
    public int round;
    public float time;
    public float scoredobj_hash;
    public string scoredobj_name;
    public int scoreID;
}


public class ScoringManager : MonoBehaviour
{
    [SerializeField] ScoreTracker blueScoreTracker;
    [SerializeField] ScoreTracker redScoreTracker;

    private static Dictionary<Team, ScoreTracker> scores = new Dictionary<Team, ScoreTracker>();

    private static ScoringReporter scoring = new ScoringReporter();

    private static GameTimeManager gametime;
    // Start is called before the first frame update
    void Start()
    {
        gametime = FindFirstObjectByType<GameTimeManager>();
        scores.Clear();
        scoring = new ScoringReporter();
        scores.Add(Team.blue, blueScoreTracker);
        scores.Add(Team.red, redScoreTracker);
    }

    public static void ScoreEvent(Team team, int amount, string reason, GameObject reportingObj)
    {
        //pass scoring type data so we can assign points based on current round
        Debug.Log($"{team} Score: {amount} for {reason} ({reportingObj})");
        scores[team].AddOrSubtractScore(amount);
        AddScore(team, amount, reason, reportingObj);
    }

    public static int AddScore(Team team, int amount, string reason, GameObject reportingObj)
    {
        GameTimeManager timemgr = FindFirstObjectByType<GameTimeManager>();
        
        ScoreEntry entry = new ScoreEntry();
        entry.reason = reason;//scoring guide name
        entry.amount = amount;
        entry.team = team;
        entry.details = reason;
        entry.time = gametime.gameTime.globalInt;
        entry.round = gametime.currentSession.globalInt;
        entry.scoredobj_hash = reportingObj.GetHashCode();
        entry.scoredobj_name = reportingObj.name;

        int index = scoring.AddScore(entry);
        scores[team].AddOrSubtractScore(entry.amount);

        return index;
    }
    public static int AddScore(Team team,ScoringGuide guide, int scoreIndex, string details, GameObject reportingObj, int amount = 1)
    {

        //will have to return both scored amt and id?
        ScoreEntry entry = createEntry(team, guide, scoreIndex, details, reportingObj, amount);

        int index = scoring.AddScore(entry);
        scores[team].AddOrSubtractScore(entry.amount);

        return index;
    }

    public static void OverwriteScore(int oldScoreIdx, Team team, ScoringGuide guide, int scoreIndex, string details, GameObject reportingObj, int amount = 1)
    {
        ScoreEntry entry = createEntry(team, guide, scoreIndex, details, reportingObj, amount);

        int origAmt = scoring.GetScore(oldScoreIdx).amount;
        int scoreDiff = entry.amount - origAmt;

        scoring.OverwriteScore(entry, oldScoreIdx);
        scores[team].AddOrSubtractScore(scoreDiff);
    }

    public static void OverwriteScore(int index, Team team, int amount, string reason, GameObject reportingObj)
    {
        GameTimeManager timemgr = FindFirstObjectByType<GameTimeManager>();
        ScoreEntry entry = scoring.GetScore(index);
        entry.amount = amount;
        entry.reason = reason;
        entry.time = timemgr.gameTime.globalInt;
        entry.round = timemgr.currentSession.globalInt;
        entry.team = team;

        int origAmt = scoring.GetScore(index).amount;
        int scoreDiff = entry.amount - origAmt;

        scoring.OverwriteScore(entry, index);
        scores[team].AddOrSubtractScore(scoreDiff);
    }

    public static ScoreEntry createEntry(Team team, ScoringGuide guide, int scoreIndex, string details, GameObject reportingObj, int amount = 1)
    {
        int round = gametime.currentSession.globalInt;

        if (round == 3) { round = 1; } // free play, but score as if tele-op.
        if (round == GameTimeManager.NoScoreRound) { Debug.Log("Game is Over, no score"); return null; }
        int scoreAmt = guide.scoresPerSessionPerType[scoreIndex].scoresPerRound[round];

        ScoreEntry entry = new ScoreEntry();
        entry.reason = guide.name;//scoring guide name
        entry.amount = amount * scoreAmt;
        entry.team = team;
        entry.details = details;
        entry.time = gametime.gameTime.globalInt;
        entry.round = gametime.currentSession.globalInt;
        entry.scoredobj_hash = reportingObj.GetHashCode();
        entry.scoredobj_name = reportingObj.name;

        return entry;
    }



    public List<ScoreEntry> GetScoreEntries()
    {
        return scoring.GetList();
    }
    public override string ToString()
    {
        string outp = "";
        foreach(ScoreEntry entry in GetScoreEntries())
        {
            outp += $"[{entry.team}]({entry.round}-{entry.time}):{entry.reason}: +{entry.amount} points from {entry.scoredobj_name}. {entry.details}\n";
        }

        return outp;
    }
}

public class ScoringReporter
{
    public Dictionary<int, ScoreEntry> scoreData = new Dictionary<int,ScoreEntry>();

    public int AddScore(ScoreEntry entry)
    {
        int idx = scoreData.Count;
        scoreData.Add(idx,entry);
        return idx;
    }

    public void OverwriteScore(ScoreEntry entry, int index)
    {
        scoreData[index] = entry;
    }

    public ScoreEntry GetScore(int index)
    {
        return scoreData[index];
    }

    public void RemoveScore(int index)
    {
        scoreData.Remove(index);
    }

    public List<ScoreEntry> GetList()
    {
        return scoreData.Values.ToList();
    }
}

