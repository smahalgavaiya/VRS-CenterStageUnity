using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scoring/Scoring Guide")]
public class ScoringGuide : ScriptableObject
{
    public RoundIndex roundIndex;
    public ScoreObjectType[] scoreObjectTypes;
    public List<ScorePerRound> scoresPerRound;
}

[System.Serializable]
public class ScorePerRound
{
    public string RoundName { get;set;}
    public int RoundNumber { get; set; }
    public int Score { get;set;}
}
