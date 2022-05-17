using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Check the object in the trigger volume to see if it matches Scoring Object Types
public class ObjectChecker : MonoBehaviour
{
    public ScoringGuide scoringGuide;
    public bool CanPickUp { get; set; }

    public GameObject ObjectInTrigger { get; set; }

    List<string> scoringObjectTypeNames;
    // Start is called before the first frame update
    void Start()
    {
        scoringObjectTypeNames = new List<string>();
        foreach(ObjectType scoreObjectType in scoringGuide.scoreObjectTypes)
        {
            scoringObjectTypeNames.Add(scoreObjectType.name);
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        // Check to see if the object is the right type
        foreach(string scoreObjectTypeName in scoringObjectTypeNames)
        {
            if (scoreObjectTypeName == other.tag)
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
        foreach(string scoreObjectTypeName in scoringObjectTypeNames)
        {
            if (scoreObjectTypeName == other.tag)
            {
                CanPickUp = false;
                ObjectInTrigger = null;
                break;
            } 
        }
    }
}
