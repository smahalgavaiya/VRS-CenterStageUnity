using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalZoneScorer : MonoBehaviour
{
    public RoundIndex roundIndex;
    public ScoreIndex scoreIndex;

    [SerializeField]
    private List<ScoringObject> scoringObjects;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

public class RoundScoreData
{
    public ScoringObject ScoringObject { get; set; }
}
