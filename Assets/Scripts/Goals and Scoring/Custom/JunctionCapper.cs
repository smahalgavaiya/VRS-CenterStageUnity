using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JunctionCapper : MonoBehaviour, ICustomGoalEvents
{
    Stack<TeamColor> objectsOnJunction;
    public bool IsCapped { get { if (objectsOnJunction.Count > 0) return true; else return false; } }
    public TeamColor CurrentCapColor { get => objectsOnJunction.Peek(); }

    [SerializeField] ScoringGuide cappedScoringGuide;

    [SerializeField] GameEvent checkCircuit;

    // Start is called before the first frame update
    void Start()
    {
        objectsOnJunction = new Stack<TeamColor>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void DoCustomOffEvent(Object objectToPass)
    {
        GoalZoneScoreLink goalZoneScoreLink = (GoalZoneScoreLink)objectToPass;
        TeamColor departingColor = objectsOnJunction.Pop();

        if (objectsOnJunction.Count > 0 && departingColor != objectsOnJunction.Peek())
        {
            goalZoneScoreLink.ChangeScore(0, cappedScoringGuide, -1, departingColor);
            goalZoneScoreLink.ChangeScore(0, cappedScoringGuide, 1, objectsOnJunction.Peek());
        }
        else if (objectsOnJunction.Count < 1)
            goalZoneScoreLink.ChangeScore(0, cappedScoringGuide, -1, departingColor);

        checkCircuit.Raise();
    }

    public void DoCustomOnEvent(Object objectToPass)
    {
        GoalZoneScoreLink goalZoneScoreLink = (GoalZoneScoreLink)objectToPass;

        if (objectsOnJunction.Count > 0)
        {
            TeamColor replacedColorInStack = objectsOnJunction.Peek();
            objectsOnJunction.Push(goalZoneScoreLink.LastObjectTeamColor);

            if (replacedColorInStack != objectsOnJunction.Peek())
            {
                goalZoneScoreLink.ChangeScore(0, cappedScoringGuide, -1, replacedColorInStack);
                goalZoneScoreLink.ChangeScore(0, cappedScoringGuide, 1, objectsOnJunction.Peek());
            }
        }
        else
        {
            objectsOnJunction.Push(goalZoneScoreLink.LastObjectTeamColor);
            goalZoneScoreLink.ChangeScore(0, cappedScoringGuide, 1, objectsOnJunction.Peek());
        }

        checkCircuit.Raise();

    }

}
