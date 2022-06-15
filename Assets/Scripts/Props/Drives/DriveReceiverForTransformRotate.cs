using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DriveReceiverForTransformRotate: DriveReceiver
{
    [SerializeField]
    float coefficientOfDrive = 1;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(drive.driveAmount * coefficientOfDrive);
    }
}
