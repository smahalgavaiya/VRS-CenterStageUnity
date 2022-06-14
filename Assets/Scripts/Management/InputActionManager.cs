using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;
using System;

public class InputActionManager : MonoBehaviour
{
    public UnityEvent pickUpObject;
    public GameObject robot;

    public Drive frontLeftWheel, backLeftWheel, frontRightWheel, backRightWheel, motor1, motor2, motor3, motor4;
    List<Drive> allDrives;
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
    }

    public void OnMovement(InputValue value)
    {
        Vector2 direction = value.Get<Vector2>();

        if (CheckRobotForMecanum())
            TranslateMecanumMotion(direction);
    }

    private bool CheckRobotForMecanum()
    {
        if (robot.GetComponentInChildren<DriveReceiverMecanum>())
            return true;
        else
            return false;
    }

    public void OnPickUpObject(InputValue value)
    {
        if (value.isPressed)
            pickUpObject.Invoke();
    }


    // Update is called once per frame
    void Update()
    {
        
        
    }

    void TranslateMecanumMotion(Vector2 direction)
    {
        // Mecanum settings
        frontLeftWheel.driveAmount.x = -direction.x + direction.y;
        backRightWheel.driveAmount.x = -direction.x + direction.y;
        frontRightWheel.driveAmount.x = direction.x + direction.y;
        backLeftWheel.driveAmount.x = direction.x + direction.y;
    }
}
