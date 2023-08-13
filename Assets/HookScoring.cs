using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HookScoring : MonoBehaviour
{
    private bool isHooked = false;
    private int scoreID = -1;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(isHooked)
        {
            GameObject bot = transform.root.gameObject;
            DistanceToFloor sensor = bot.GetComponentInChildren<DistanceToFloor>();
            Team t = (Team)bot.GetComponent<ScoreObjectTypeLink>().LastTouchedTeamColor;
            if(sensor != null)
            {
                if(!sensor.isGrounded && scoreID == -1)
                {
                    //restrict to endgame
                    GameTimeManager gametime = FindFirstObjectByType<GameTimeManager>();
                    if(gametime.currentSession.globalInt > 1)
                    {
                        scoreID = ScoringManager.AddScore(t, 20, "Robot Suspended", bot);
                    }
                    
                }
            }
        }
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag.ToLower() == "rigging")
        {
            isHooked = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag.ToLower() == "rigging")
        {
            isHooked = false;
        }
    }
}
