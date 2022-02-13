using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Props/Driver Link")]
public class Driver : ScriptableObject
{
    List<DriverReceiver> driverReceivers;
    public Vector3 driveAmount;
    public DriverType driverType;
    private void OnEnable()
    {
        driverReceivers = new List<DriverReceiver>();
    }

    public void RegisterDriverReceiver(DriverReceiver driverReceiver)
    {
        driverReceivers.Add(driverReceiver);
    }

    public void UnRegisterDriverReceiver(DriverReceiver driverReceiver)
    {
        driverReceivers.Remove(driverReceiver);
    }

    public void ActivateDriver()
    {
        // We go backwards so if we un-register one, it doesn't fowl up the list
        for (int i = driverReceivers.Count - 1; i > -1; i--)
        {
            driverReceivers[i].EnableDrive();
        }
    }
    public void DeActivateDriver()
    {
        // We go backwards so if we un-register one, it doesn't fowl up the list
        for (int i = driverReceivers.Count - 1; i > -1; i--)
        {
            driverReceivers[i].DisableDrive();
        }
    }
}

public enum DriverType
{
    Rotation,
    Translation,
    Force,
    Torque,
    RigidbodyMove,
    RigidbodyRotate
}
