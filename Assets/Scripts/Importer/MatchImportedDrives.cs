using System.Collections;
using System.Collections.Generic;

// This class matches the imported robot drives to the existing drives, e.g. imported motor1.asset to existing motor1.asset
public static class MatchImportedDrives  
{
    public static void MatchDrives(InputActionManager inputActionManager, DriveIndex existingDriveIndex)
    {
        inputActionManager.frontLeftWheel = existingDriveIndex.frontLeftWheel;
        inputActionManager.backLeftWheel = existingDriveIndex.backLeftWheel;
        inputActionManager.frontRightWheel = existingDriveIndex.frontRightWheel;
        inputActionManager.backRightWheel = existingDriveIndex.backRightWheel;
        inputActionManager.motor1 = existingDriveIndex.motor1;
        inputActionManager.motor2 = existingDriveIndex.motor2;
        inputActionManager.motor3 = existingDriveIndex.motor3;
        inputActionManager.motor4 = existingDriveIndex.motor4;
    }
}
