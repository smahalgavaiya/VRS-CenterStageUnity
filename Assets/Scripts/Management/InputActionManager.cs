using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;
using System;

public class InputActionManager : MonoBehaviour
{
    public GameObject robot;

    public Drive frontLeftWheel, backLeftWheel, frontRightWheel, backRightWheel, motor1, motor2, motor3, motor4;
    List<Drive> allDrives;

    bool robotHasMecanumWheels;
    private Vector2 moveDirection;
    private float rotationDirection;

    // Start is called before the first frame update
    void Start()
    {
        allDrives = new List<Drive>();

        allDrives.Add(frontLeftWheel);
        allDrives.Add(backLeftWheel);
        allDrives.Add(frontRightWheel);
        allDrives.Add(backRightWheel);
        allDrives.Add(motor1);
        allDrives.Add(motor2);
        allDrives.Add(motor3);
        allDrives.Add(motor4);
        
        foreach(Drive drive in allDrives)
        {
            drive.driveAmount = Vector3.zero;
        }

        if (robot.GetComponentInChildren<DriveReceiverMecanum>())
            robotHasMecanumWheels = true;
        else
            robotHasMecanumWheels = false;
    }

    public void OnMovement(InputValue value)
    {
        moveDirection = value.Get<Vector2>();

        if (robotHasMecanumWheels)
        {
            MecanumMotion();
        }
    }

    public void OnRotation(InputValue value)
    {
        rotationDirection = value.Get<float>();

        if (robotHasMecanumWheels)
        {
            MecanumMotion();
        }
    }
    
    public void OnMotorOne(InputValue value)
    {
        motor1.driveAmount.x = value.Get<float>();
    }

    public void OnMotorTwo(InputValue value)
    {
        motor2.driveAmount.x = value.Get<float>();
    }

    // Update is called once per frame
    void Update()
    {
        
        
    }

    void MecanumMotion()
    {
        // Mecanum settings
        frontLeftWheel.driveAmount.x = -moveDirection.x + moveDirection.y + rotationDirection;
        backRightWheel.driveAmount.x = -moveDirection.x + moveDirection.y - rotationDirection;
        frontRightWheel.driveAmount.x = moveDirection.x + moveDirection.y - rotationDirection;
        backLeftWheel.driveAmount.x = moveDirection.x + moveDirection.y + rotationDirection;
    }
}
