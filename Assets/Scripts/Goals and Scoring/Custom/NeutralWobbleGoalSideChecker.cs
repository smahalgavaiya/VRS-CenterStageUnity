using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeutralWobbleGoalSideChecker : MonoBehaviour
{
    BoxCollider boxCollider;
    bool correctObjectInCollider;
    [SerializeField]
    GoalZoneScoreLink goalZoneScoreLink;
    [SerializeField] ScoreZoneColor teamColor;
    // Start is called before the first frame update
    void Start()
    {
        boxCollider = GetComponent<BoxCollider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<ScoreObjectTypeLink>())
        {
            if (teamColor == other.GetComponent<ScoreObjectTypeLink>().LastTouchedTeamColor)
                goalZoneScoreLink.OptionalBoolValue = true;
            else
                goalZoneScoreLink.OptionalBoolValue = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
