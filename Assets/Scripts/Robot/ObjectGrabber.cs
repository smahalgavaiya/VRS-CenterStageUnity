using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

[RequireComponent(typeof(ObjectChecker))]
public class ObjectGrabber : MonoBehaviour
{
    ObjectChecker objectChecker;
    bool isHoldingObject = false;
    GameObject heldObject;
    Transform originalTransformParent;

    Rigidbody heldObjectRigidBody;

    // Start is called before the first frame update
    void Start()
    {
        objectChecker = GetComponent<ObjectChecker>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isHoldingObject)
        {
            heldObjectRigidBody.MovePosition(transform.position);
            heldObjectRigidBody.MoveRotation(transform.rotation);
        }
    }


    public void PickUpOrPutDownObject()
    {
        if (objectChecker.CanPickUp && !isHoldingObject)
        {
            heldObject = objectChecker.ObjectInTrigger;
            heldObjectRigidBody = heldObject.GetComponent<Rigidbody>();
            heldObjectRigidBody.ResetInertiaTensor();
            heldObjectRigidBody.useGravity = false;
            isHoldingObject = true;
            objectChecker.enabled = false;
        }

        else if (isHoldingObject)
        {
            isHoldingObject = false;
            heldObjectRigidBody.ResetInertiaTensor();
            heldObjectRigidBody.useGravity = true;
        }

    }
}
