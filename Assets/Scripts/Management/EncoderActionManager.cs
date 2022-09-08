using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EncoderActionManager : MonoBehaviour
{
    [Range(1.0f,20.0f)]
    public float forceMultiplier = 3f;
    public Drive frontLeftWheel, backLeftWheel, frontRightWheel, backRightWheel, motor1, motor2, motor3, motor4;
    List<Drive> drives;
    // Start is called before the first frame update
    void Start()
    {
        drives = new List<Drive>();

        drives.Add(frontLeftWheel);
        drives.Add(backLeftWheel);
        drives.Add(frontRightWheel);
        drives.Add(backRightWheel);
        drives.Add(motor1);
        drives.Add(motor2);
        drives.Add(motor3);
        drives.Add(motor4);
        foreach(Drive drive in drives)
        {
            if(drive != null)
                drive.driveAmount = Vector3.zero;
        }
    }
    public void SetFrontLeft(float driveAmt)
    {
        frontLeftWheel.driveAmount = new Vector3(-driveAmt * forceMultiplier,0,0);
    }
    public void SetFrontRight(float driveAmt)
    {
        frontRightWheel.driveAmount = new Vector3(-driveAmt * forceMultiplier,0,0);
    }
    public void SetBackLeft(float driveAmt)
    {
        backLeftWheel.driveAmount = new Vector3(-driveAmt * forceMultiplier,0,0);
    }
    public void SetBackRight(float driveAmt)
    {
        backRightWheel.driveAmount = new Vector3(-driveAmt * forceMultiplier,0,0);
    }
    public void SetMotor1(float driveAmt)
    {
        motor1.driveAmount.x = -driveAmt;
    }
    public void SetMotor2(float driveAmt)
    {
        motor2.driveAmount.x = driveAmt;
    }
}
