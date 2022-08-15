using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckRobotWithinBounds : MonoBehaviour, ICustomGoalChecker
{
    GoalZoneScoreLink goalZoneScoreLink;
    GameObject goalBoundsObject;
    CapsuleCollider goalBounds;

    // Start is called before the first frame update
    void Start()
    {
        goalBoundsObject = this.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void DoCustomCheck()
    {
    }

    public void DoCustomCheck(GameObject objectToCheck)
    {
        if (CheckRobotBounds(objectToCheck))
            goalZoneScoreLink.OptionalBoolValue = true;
        else 
            goalZoneScoreLink.OptionalBoolValue = false;
    }

    private bool CheckRobotBounds(GameObject objectToCheck)
    {
        Collider objectToCheckCollider = objectToCheck.GetComponent<Collider>();

        List<Vector3> pointsToCheck = new List<Vector3>();
        pointsToCheck.Add(objectToCheckCollider.bounds.center + objectToCheckCollider.bounds.extents);
        pointsToCheck.Add(objectToCheckCollider.bounds.center - objectToCheckCollider.bounds.extents);

        bool containsObject = false;

        foreach(Vector3 vector3 in pointsToCheck)
        {
            if (goalBounds.bounds.Contains(vector3))
                containsObject = true;
            else
            {
                containsObject = false;
                break;
            }
        }

        return containsObject;
    }
}
