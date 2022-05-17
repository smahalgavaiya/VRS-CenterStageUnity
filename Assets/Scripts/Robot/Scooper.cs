using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ObjectChecker))]
public class Scooper : MonoBehaviour
{
    ObjectChecker objectChecker;
    bool isHoldingObject = false;
    GameObject heldObject;
    Transform originalTransformParent;

    // Start is called before the first frame update
    void Start()
    {
        objectChecker = GetComponent<ObjectChecker>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void PickUpOrPutDownObject()
    {
        if (objectChecker.CanPickUp && !isHoldingObject)
        {
            if (objectChecker.ObjectInTrigger.GetComponent<Rigidbody>() != null)
            {
                objectChecker.ObjectInTrigger.GetComponent<Rigidbody>().collisionDetectionMode = 
                    CollisionDetectionMode.ContinuousSpeculative;
                objectChecker.ObjectInTrigger.GetComponent<Rigidbody>().isKinematic = true;
            }
            heldObject = objectChecker.ObjectInTrigger;
            originalTransformParent = heldObject.transform.parent;
            heldObject.transform.SetParent(transform);
            heldObject.transform.position += new Vector3(0, .1f, 0);

            isHoldingObject = true;
            objectChecker.enabled = false;
        }

        else if (isHoldingObject)
        {
            heldObject.transform.SetParent(originalTransformParent);
            
            if (heldObject.GetComponent<Rigidbody>() != null)
            {
                heldObject.GetComponent<Rigidbody>().isKinematic = false;
            }

            isHoldingObject = false;

        }

    }
}
