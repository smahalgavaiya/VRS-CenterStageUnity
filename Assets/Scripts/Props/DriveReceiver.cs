using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DriveReceiver : MonoBehaviour
{
    public Drive drive;
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
    public void ReceiveDriveValue(float value)
    {
        driveCoefficient = value;
    }
}
