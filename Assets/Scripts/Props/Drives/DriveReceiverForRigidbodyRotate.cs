using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class DriveReceiverForRigidbodyRotate : DriveReceiver
{
    Rigidbody rigidbody_;
    // Start is called before the first frame update
    void Start()
    {
        rigidbody_ = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        rigidbody_.MoveRotation(transform.rotation * Quaternion.Euler(drive.driveAmount));
    }
}
