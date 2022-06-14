using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DriveReceiver : MonoBehaviour
{
    public Drive drive;
    bool driveIsActive = false;

    private void Awake()
    {
        if (drive != null)
            drive.RegisterDriveReceiver(this);    
    }

    private void OnDestroy()
    {
        if (drive != null)
            drive.UnRegisterDriveReceiver(this);
    }
}
