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

    public TeamColor scoreZoneColor;

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

        if (scoreZoneColor == TeamColor.Blue)
        {
            material = materialIndex.blueGoalMaterial;
            ScoreTracker = scoreTrackerIndex.blueScoreTracker;
        }
        else if (scoreZoneColor == TeamColor.Red)
        {
            material = materialIndex.redGoalMaterial;
            ScoreTracker = scoreTrackerIndex.redScoreTracker;
        }
        else 
        {
            material = materialIndex.eitherGoalMaterial;
            ScoreTracker = scoreTrackerIndex.redScoreTracker;
        }

        GetComponent<GoalZoneColorSwitcher>().SetColor(material);

        // Set tape color if the Goal Zone Tape Maker is available
        if (GetComponent<GoalZoneTapeMaker>())
            GetComponent<GoalZoneTapeMaker>().SetTapeColor(scoreZoneColor);

    }
}

public enum TeamColor
{
    Blue,
    Red,
    Either
}

