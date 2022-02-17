using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scoring/Scoring Guide")]
public class ScoringGuide : ScriptableObject
{
    public RoundIndex roundIndex;
    public ScoreObjectType[] scoreObjectTypes;
    public ScorePerRoundPerType[] scoresPerRoundPerType;
}

[System.Serializable]
public class ScorePerRoundPerType
{
    public int[] scoresPerRound;
}
