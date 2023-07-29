using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceRandomizationObject : MonoBehaviour, ICustomGoalChecker
{
    public Team owningTeam;
    public GameObject objectPrefab;
    public Dictionary<string, Transform> randomizationLocations = new Dictionary<string, Transform>();


    private Collider[] colliders;
    private string pickedRandomization = "none";

    public bool leaveObjAsChild = false;//leave object as a child of the creating object.
    // Start is called before the first frame update

    //get all instances of placeR_obj, have a master class that will set randomization and wait on an event from field manager.
    //

    private void Awake()
    {
        colliders = GetComponentsInChildren<Collider>();
        randomizationLocations = new Dictionary<string, Transform>();
        foreach (Transform t in transform)
        {
            randomizationLocations.Add(t.name, t);
        }
    }

    public void Place(string loc)
    {
        Awake();
        if(randomizationLocations.ContainsKey(loc))
        {
            GameObject obj = GameObject.Instantiate(objectPrefab);
            obj.transform.position = randomizationLocations[loc].position;
            obj.transform.rotation = randomizationLocations[loc].rotation;
            pickedRandomization = loc;
            if (leaveObjAsChild) { obj.transform.parent = transform; }
            //obj.transform.parent = transform;
        }
    }

    public GameObject GetCallingTrigger(GameObject objToCheck)
    {
        Collider other = objToCheck.GetComponent<Collider>();
        foreach(Collider c in colliders)
        {
            if(c.bounds.Intersects(other.bounds))
            {
                return c.gameObject;
            }
        }
        return null;
    }

    public void DoCustomCheck()
    {
        
    }

    public void DoCustomCheck(GameObject objectToCheck, int scoreDirection)
    {
        GameObject triggeringObj = GetCallingTrigger(objectToCheck);
        GoalZoneScoreLink goal = GetComponent<GoalZoneScoreLink>();
        if(triggeringObj == null) { return; }
        if(triggeringObj.name == pickedRandomization || triggeringObj.transform.parent.name == pickedRandomization)
        {
            goal.OptionalBoolValue = true;
        }
        else { goal.OptionalBoolValue = false; }
        //Debug.Log(objectToCheck.name);
    }
}
