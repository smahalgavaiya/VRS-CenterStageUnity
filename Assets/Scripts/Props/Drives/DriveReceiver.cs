using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DriveReceiver : MonoBehaviour
{
    public Drive drive;
    bool driveIsActive = false;

    private void Awake()
    {
        drive.RegisterDriveReceiver(this);    
    }

    private void OnDestroy()
    {
        drive.UnRegisterDriveReceiver(this);
    }
}
