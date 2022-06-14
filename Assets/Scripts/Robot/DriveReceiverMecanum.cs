using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DriveReceiverMecanum : DriveReceiver
{
    [SerializeField]
    Drive frontLeft, frontRight, backLeft, backRight;

    float coefficientOfMotion = .0005f;

    Vector3 movementDirection = Vector3.zero;
    float rotationDirection = 0;

    Vector3 CalculateDirection()
    {
        movementDirection = new Vector2(frontLeft.driveAmount.x - frontRight.driveAmount.x - backLeft.driveAmount.x + backRight.driveAmount.x,
            frontLeft.driveAmount.x + frontRight.driveAmount.x + backLeft.driveAmount.x + backRight.driveAmount.x);

        movementDirection = new Vector3(-movementDirection.x, 0, movementDirection.y);// TODO fix this

        return movementDirection;
    }

    float CalculateRotation()
    {
        rotationDirection = frontLeft.driveAmount.x + backLeft.driveAmount.x - frontRight.driveAmount.x - backRight.driveAmount.x;

        return rotationDirection;
    }

    private void Update()
    {
        transform.Translate(CalculateDirection() * coefficientOfMotion, Space.Self);

        transform.Rotate(new Vector3(0, 0, CalculateRotation() * coefficientOfMotion));
    }


}
