using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectLauncher : MonoBehaviour
{
    public bool IsHoldingObject { get; set; }
    public Drive launchDrive;
    public GameObject HeldObject { get; set; }
    public float launchForceCoefficient = 5;
    public bool CheckObjectGrabber { get; set; }
    public ObjectGrabber ObjectGrabber_ { get; set; }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (launchDrive.driveAmount.x > 0 && IsHoldingObject)
        {
            if (CheckObjectGrabber)
                ObjectGrabber_.PickUpOrPutDownObject();
            float launchForce = launchDrive.driveAmount.x;
            Rigidbody heldObjectRB = HeldObject.GetComponent<Rigidbody>();
            heldObjectRB.AddForce(transform.forward * launchForce * launchForceCoefficient, ForceMode.Impulse);
            IsHoldingObject = false;
        }
    }


}
