using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

//scoring for an object landing in a number of zones. but only allow object to be scored once.
public class AreaScorer : MonoBehaviour, ICustomGoalChecker
{
    private List<Collider> colliders = new List<Collider>();
    private Dictionary<TeamColor, bool> hasScored = new Dictionary<TeamColor, bool>();
    // Start is called before the first frame update
    void Start()
    {
        colliders = GetComponentsInChildren<Collider>().ToList();
        hasScored[TeamColor.Red] = false;
        hasScored[TeamColor.Blue] = false;
    }

    public List<GameObject> GetCallingTriggers(GameObject objToCheck)
    {
        Collider other = objToCheck.GetComponent<Collider>();
        List<GameObject> triggers = new List<GameObject>();
        foreach (Collider c in colliders)
        {
            if (c.bounds.Intersects(other.bounds))
            {
                triggers.Add(c.gameObject);
            }
        }
        return triggers;
    }

    public void DoCustomCheck(GameObject objectToCheck, int scoreDirection)
    {
        List<GameObject> triggeringObjs = GetCallingTriggers(objectToCheck);
        GoalZoneScoreLink goal = GetComponent<GoalZoneScoreLink>();
        ScoreObjectTypeLink obj = objectToCheck.transform.root.GetComponent<ScoreObjectTypeLink>();
        foreach (GameObject trigger in triggeringObjs)
        {
            GoalZoneScoreLink scoring = trigger.GetComponent<GoalZoneScoreLink>();
            if(hasScored[obj.LastTouchedTeamColor])
            {
                if(scoring)
                {
                    scoring.OptionalBoolValue = false;
                }
            }
            else if (scoring)
            {
                hasScored[obj.LastTouchedTeamColor] = true;
                scoring.OptionalBoolValue = true;
                break;
            }
        }

    }

    public void DoCustomCheck()
    {

    }
}
