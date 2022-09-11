using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EncoderActionManager : MonoBehaviour
{
    [Range(1.0f,20.0f)]
    public float forceMultiplier = 3f;
    public Drive frontLeftWheel, backLeftWheel, frontRightWheel, backRightWheel, motor1, motor2, motor3, motor4;
    List<Drive> drives;

    private Vector2 moveDirection;
    private float rotationDirection;
    public InputActionManager gamepadControls;

    float FL, FR, RL, RR = 0;
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
        FL = -driveAmt * forceMultiplier;
    }
    public void SetFrontRight(float driveAmt)
    {
        FR = -driveAmt * forceMultiplier;
    }
    public void SetBackLeft(float driveAmt)
    {
        RL = -driveAmt * forceMultiplier;
    }
    public void SetBackRight(float driveAmt)
    {
        RR = -driveAmt * forceMultiplier;
    }
    public void SetMotor1(float driveAmt)
    {
        if (gamepadControls.GamepadActive) { return; }
        motor1.driveAmount.x = -driveAmt;
    }
    public void SetMotor2(float driveAmt)
    {
        if (gamepadControls.GamepadActive) { return; }
        motor2.driveAmount.x = driveAmt;
    }

    void WheelTorqueMotion()
    {
        if (gamepadControls.GamepadActive) { return; }
        //EX lateral: FL:1,FR:-1,RL:-1,RR:1
        moveDirection.y = FL + FR + RL + RR;
        moveDirection.x = FL - FR - RL + RR;
        rotationDirection = FL - FR + RL - RR;
        moveDirection.y = Mathf.Clamp(moveDirection.y, -1, 1);
        moveDirection.x = Mathf.Clamp(moveDirection.x, -1, 1);
        rotationDirection = Mathf.Clamp(rotationDirection, -1, 1);
        float forward = moveDirection.y;
        float strafe = moveDirection.x;
        float rotate = rotationDirection;
        for (int i = 0; i < 4; i++)
        {
            drives[i].driveAmount = new Vector3(forward, strafe, rotate);
        }
        frontLeftWheel.driveAmount.x = -moveDirection.x + moveDirection.y + rotationDirection;
        backRightWheel.driveAmount.x = -moveDirection.x + moveDirection.y - rotationDirection;
        frontRightWheel.driveAmount.x = moveDirection.x + moveDirection.y - rotationDirection;
        backLeftWheel.driveAmount.x = moveDirection.x + moveDirection.y + rotationDirection;

    }

    private void Update()
    {
        //FL = RR = 1;
        //FR = RL = -1;
        WheelTorqueMotion();
    }
}
