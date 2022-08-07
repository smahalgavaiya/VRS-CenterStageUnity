using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JunctionCapper : MonoBehaviour, ICustomGoalEvents
{
    Stack<TeamColor> objectsOnJunction;
    public Stack<TeamColor> ObjectsOnJunction { get => objectsOnJunction; set => objectsOnJunction = value; }

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
        objectsOnJunction.Pop();
    }

    public void DoCustomOnEvent(Object objectToPass)
    {
        GoalZoneScoreLink goalZoneScoreLink = (GoalZoneScoreLink)objectToPass;
        objectsOnJunction.Push(goalZoneScoreLink.LastObjectTeamColor);
    }

}
