using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DriveReceiver : MonoBehaviour
{
    public Drive drive;
    Rigidbody rigidbody;
    bool driveIsActive = false;
    float driveCoefficient = 0;

    private void Awake()
    {
        drive.RegisterDriveReceiver(this);    
    }

    private void OnDestroy()
    {
        drive.UnRegisterDriveReceiver(this);
    }

    public void RegisterRigidbody()
    {
        try
        {
            if (GetComponent<Rigidbody>() == null)
                throw new System.Exception("You are trying to access a Rigidbody, but this object does not have one!");
            rigidbody = GetComponent<Rigidbody>();
        }
        catch
        {
            Debug.Log("You're trying to access a rigidbody on " + this.name + ", but it doesn't have one!");
            Debug.Break();
        }
    }

    public void ReceiveDriveValue(float value)
    {
        driveCoefficient = value;
    }

    private void Update()
    {
        switch (drive.driverType)
        {
            case DriverType.Rotation:
                transform.Rotate(drive.driveAmount * driveCoefficient);
                break;
            case DriverType.Translation:
                transform.Translate(drive.driveAmount * driveCoefficient);
                break;
            case DriverType.RigidbodyMove:
                rigidbody.MovePosition(rigidbody.position + drive.driveAmount * driveCoefficient);
                break;
            case DriverType.RigidbodyRotate:
                rigidbody.MoveRotation(rigidbody.rotation * Quaternion.Euler(drive.driveAmount * driveCoefficient));
                break;
            case DriverType.Force:
                rigidbody.AddForce(drive.driveAmount * driveCoefficient);
                break;
            case DriverType.Torque:
                rigidbody.AddTorque(drive.driveAmount * driveCoefficient);
                break;
        }
    }
}
