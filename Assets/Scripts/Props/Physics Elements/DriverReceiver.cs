using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DriverReceiver : MonoBehaviour
{
    public Driver driverLink;
    Rigidbody rigidbody;
    bool driveIsActive = false;

    private void Awake()
    {
        driverLink.RegisterDriverReceiver(this);    
    }

    private void OnDestroy()
    {
        driverLink.UnRegisterDriverReceiver(this);
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

    public void EnableDrive()
    {
        driveIsActive = true;
    }
    public void DisableDrive()
    {
        driveIsActive = false;
    }

    private void Update()
    {
        if (driveIsActive)
        {
            switch (driverLink.driverType)
            {
                case DriverType.Rotation:
                    transform.Rotate(driverLink.driveAmount);
                    break;
                case DriverType.Translation:
                    transform.Translate(driverLink.driveAmount);
                    break;
                case DriverType.RigidbodyMove:
                    rigidbody.MovePosition(rigidbody.position + driverLink.driveAmount);
                    break;
                case DriverType.RigidbodyRotate:
                    rigidbody.MoveRotation(rigidbody.rotation * Quaternion.Euler(driverLink.driveAmount));
                    break;
                case DriverType.Force:
                    rigidbody.AddForce(driverLink.driveAmount);
                    break;
                case DriverType.Torque:
                    rigidbody.AddTorque(driverLink.driveAmount);
                    break;
            }
        }
    }
}
