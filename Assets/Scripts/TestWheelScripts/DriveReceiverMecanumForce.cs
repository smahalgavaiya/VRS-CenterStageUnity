using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class DriveReceiverMecanumForce : DriveReceiver
{
    [SerializeField]
    protected Drive frontLeft, frontRight, backLeft, backRight;

    public Drive FrontLeft { set { frontLeft = value; } }
    public Drive FrontRight { set { frontRight = value; } }
    public Drive BackLeft { set { backLeft = value; } }
    public Drive BackRight { set { backRight = value; } }

    protected Rigidbody rigidbody;

    public float rotationForce = 1000;
    public float moveForce = 3000;

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

    private void FixedUpdate()
    {

        Vector3 dir = CalculateDirection();
        //dir.y = dir.z;
        //dir.z = 0;
        rigidbody.AddRelativeForce(dir * moveForce);
        Vector3 rot = new Vector3(0, CalculateRotation() * rotationForce, 0);
        rigidbody.AddRelativeTorque(rot,ForceMode.Impulse);
    }

    /*private void driveRobot()
    {
        if (rb == null)
        {
            return;
        }
        // Strafer Drivetrain Control
        var linearVelocityX = ((frontLeftWheelCmd + frontRightWheelCmd + backLeftWheelCmd + backRightWheelCmd) / 1.5f) * ((motorRPM / 60) * 2 * wheelRadius * Mathf.PI);
        var linearVelocityY = ((-frontLeftWheelCmd + frontRightWheelCmd + backLeftWheelCmd - backRightWheelCmd) / 1.5f) * ((motorRPM / 60) * 2 * wheelRadius * Mathf.PI);
        var angularVelocity = (((-frontLeftWheelCmd + frontRightWheelCmd - backLeftWheelCmd + backRightWheelCmd) / 3) * ((motorRPM / 60) * 2 * wheelRadius * Mathf.PI) / (Mathf.PI * wheelSeparationWidth)) * 2 * Mathf.PI;
        // Apply Local Velocity to Rigid Body        
        var locVel = transform.InverseTransformDirection(rb.velocity);
        locVel.x = -linearVelocityY;
        locVel.z = linearVelocityX;
        locVel.y = 0f;
        rb.velocity = transform.TransformDirection(locVel);
        //Apply Angular Velocity to Rigid Body
        rb.angularVelocity = new Vector3(0f, -angularVelocity, 0f);

        if (angularVelocity == 0)
        {
            rb.freezeRotation = true;
        }
        else
        {
            rb.freezeRotation = false;
        }


        //Quaternion deltaRotation = Quaternion.Euler(new Vector3(-angularVelocity, -angularVelocity, -angularVelocity) * Time.fixedDeltaTime);
        //rb.rotation = (rb.rotation * deltaRotation);

        //Encoder Calculations 
        frontLeftWheelEnc += (motorRPM / 60) * frontLeftWheelCmd * Time.deltaTime * encoderTicksPerRev * drivetrainGearRatio;
        frontRightWheelEnc += (motorRPM / 60) * frontRightWheelCmd * Time.deltaTime * encoderTicksPerRev * drivetrainGearRatio;
        backLeftWheelEnc += (motorRPM / 60) * backLeftWheelCmd * Time.deltaTime * encoderTicksPerRev * drivetrainGearRatio;
        backRightWheelEnc += (motorRPM / 60) * backRightWheelCmd * Time.deltaTime * encoderTicksPerRev * drivetrainGearRatio;
    }*/
}
