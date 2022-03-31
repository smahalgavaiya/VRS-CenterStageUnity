using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Props/Switch")]
public class Switch : ScriptableObject
{
    List<SwitchReceiver> switchReceivers;
    public Vector3 driveAmount;
    private void OnEnable()
    {
        switchReceivers = new List<SwitchReceiver>();
    }

    public void RegisterSwitchReceiver(SwitchReceiver switchReceiver)
    {
        switchReceivers.Add(switchReceiver);
    }

    public void UnRegisterSwitchReceiver(SwitchReceiver switchReceiver)
    {
        switchReceivers.Remove(switchReceiver);
    }

    public void SendValue(bool thisValue)
    {
        // We go backwards so if we un-register one, it doesn't fowl up the list
        for (int i = switchReceivers.Count - 1; i > -1; i--)
        {
            switchReceivers[i].ReceiveSwitchValue(thisValue);
        }
    }
}
