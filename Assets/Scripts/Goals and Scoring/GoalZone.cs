using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(GoalZoneColorSwitcher))]
public class GoalZone : MonoBehaviour
{
    public MaterialIndex materialIndex;
    public ScoreIndex scoreIndex;
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
        if (scoreZone == ScoreZone.blue)
        {
            material = materialIndex.blueGoalMaterial;
            ScoreTracker = scoreIndex.blueScoreTracker;
        }
        else
        {
            material = materialIndex.redGoalMaterial;
            ScoreTracker = scoreIndex.redScoreTracker;
        }

        GetComponent<GoalZoneColorSwitcher>().SetColor(material);
        GetComponent<GoalZoneTapeMaker>().SetTapeColor(scoreZone);


    }
}

public enum ScoreZone
{
    blue,
    red
}
