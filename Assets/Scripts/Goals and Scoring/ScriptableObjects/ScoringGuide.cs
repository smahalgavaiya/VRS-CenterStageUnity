using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Goals/Scoring Guide")]
public class ScoringGuide : ScriptableObject
{
    public string objectTypesFolder = "ObjectTypes";
    public RoundIndex roundIndex;
    public ObjectType[] scoreObjectTypes;
    public ScorePerRoundPerType[] scoresPerRoundPerType;
}

[System.Serializable]
public class ScorePerRoundPerType
{
    public int[] scoresPerRound;
}
