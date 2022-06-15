using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class DriveReceiverMecanum : DriveReceiver
{
    [SerializeField]
    Drive frontLeft, frontRight, backLeft, backRight;

    Rigidbody rigidbody;

    float coefficientOfMotion = .005f;

    Vector3 movementDirection = Vector3.zero;
    float rotationDirection = 0;

    private void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
    }
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
        rigidbody.MovePosition((transform.TransformDirection(CalculateDirection()) * coefficientOfMotion) +
            transform.position);

        rigidbody.MoveRotation(Quaternion.AngleAxis(CalculateRotation(), transform.up) * transform.rotation);

        //transform.Translate(CalculateDirection() * coefficientOfMotion, Space.Self); 
        //transform.Rotate(new Vector3(0, 0, CalculateRotation() * coefficientOfMotion));
    }


}
