using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Check the object in the trigger volume to see if it matches Scoring Object Types
public class ObjectChecker : MonoBehaviour
{
    public ScoringGuide scoringGuide;
    public bool CanPickUp { get; set; }

    public GameObject ObjectInTrigger { get; set; }

    List<ObjectType> scoringObjectTypes;
    // Start is called before the first frame update
    void Start()
    {
        scoringObjectTypes = new List<ObjectType>();
        foreach(ObjectType scoreObjectType in scoringGuide.scoreObjectTypes)
        {
            if (scoreObjectType.isScoringObject)
                scoringObjectTypes.Add(scoreObjectType);
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        // Check to see if the object is the right type
        foreach(ObjectType scoreObjectType in scoringObjectTypes)
        {
            if (other.GetComponent<ScoreObjectTypeLink>() != null && 
                scoreObjectType == other.GetComponent<ScoreObjectTypeLink>().ScoreObjectType_)
            {
                CanPickUp = true;
                ObjectInTrigger = other.gameObject;
                break;
            } else
                CanPickUp = false;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        foreach(ObjectType scoreObjectType in scoringObjectTypes)
        {
            if (other.GetComponent<ScoreObjectTypeLink>() != null && 
                scoreObjectType == other.GetComponent<ScoreObjectTypeLink>().ScoreObjectType_)
            {
                CanPickUp = false;
                ObjectInTrigger = null;
                break;
            } 
        }
    }
}
