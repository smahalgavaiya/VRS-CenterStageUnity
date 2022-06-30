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

    [SerializeField]
    Drive objectGrabberDrive;

    Rigidbody heldObjectRigidBody;

    [Tooltip("Enable this if the grabber does not hold objects at its pivot point, but instead holds" +
        "them in a custom location (for instance in the scoop of a launcher")]
    [SerializeField]
    bool hasCustomHoldingLocation;
    public bool HasCustomHoldingLocation { get => hasCustomHoldingLocation; }

    [SerializeField]
    bool loadObjectLauncher;
    public bool LoadObjectLauncher { get => loadObjectLauncher; }

    [SerializeField]
    GameObject customHoldingLocation;
    public GameObject CustomHoldingLocation { get => customHoldingLocation; }

    [SerializeField]
    ObjectLauncher objectLauncher;
    public ObjectLauncher ObjectLauncher { get => objectLauncher; }


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
            switch (hasCustomHoldingLocation)
            {
                case true:
                    heldObjectRigidBody.MovePosition(customHoldingLocation.transform.position);
                    heldObjectRigidBody.MoveRotation(customHoldingLocation.transform.rotation);
                    break;
                case false:
                    heldObjectRigidBody.MovePosition(transform.position);
                    heldObjectRigidBody.MoveRotation(transform.rotation);
                    break;
            }
        }

        if (objectGrabberDrive.driveAmount.x > 0 && !isHoldingObject || 
            objectGrabberDrive.driveAmount.x == 0 && isHoldingObject)
        {
            PickUpOrPutDownObject();
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
            if (loadObjectLauncher)
            {
                objectLauncher.IsHoldingObject = true;
                objectLauncher.HeldObject = heldObject;
            }
            objectChecker.enabled = false;
        }

        else if (isHoldingObject)
        {
            isHoldingObject = false;
            if (loadObjectLauncher)
            {
                objectLauncher.IsHoldingObject = false;
                objectLauncher.HeldObject = heldObject;
            }
            heldObjectRigidBody.ResetInertiaTensor();
            heldObjectRigidBody.useGravity = true;
        }

    }
}
