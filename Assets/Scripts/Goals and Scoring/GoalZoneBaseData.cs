using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(GoalZoneColorSwitcher))]
public class GoalZoneBaseData : MonoBehaviour
{
    public MaterialIndex materialIndex;
    public ScoreTrackerIndex scoreTrackerIndex;
    public ScoringGuide scoringGuide;

    public ScoreTracker ScoreTracker { get; set; }

    public bool hideOnPlay;

    public ScoreZone scoreZone;

    // Start is called before the first frame update
    void Start()
    {
        if (hideOnPlay)
        {
            GetComponent<MeshRenderer>().enabled = false;
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnValidate()
    {
        Material material;

        if (scoreZone == ScoreZone.Blue)
        {
            material = materialIndex.blueGoalMaterial;
            ScoreTracker = scoreTrackerIndex.blueScoreTracker;
        }
        else
        {
            material = materialIndex.redGoalMaterial;
            ScoreTracker = scoreTrackerIndex.redScoreTracker;
        }

        GetComponent<GoalZoneColorSwitcher>().SetColor(material);
        GetComponent<GoalZoneTapeMaker>().SetTapeColor(scoreZone);

    }
}

public enum ScoreZone
{
    Blue,
    Red,
    Either
}

