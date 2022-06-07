using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class DriveReceiverForRigidbodyMove: DriveReceiver
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
        rigidbody_.MovePosition(transform.position + drive.driveAmount);
    }
}
