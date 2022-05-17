using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchReceiver : MonoBehaviour
{
    public Switch thisSwitch;
    bool switchIsActive = false;

    private void Awake()
    {
        thisSwitch.RegisterSwitchReceiver(this);    
    }

    private void OnDestroy()
    {
        thisSwitch.UnRegisterSwitchReceiver(this);
    }
    public void ReceiveSwitchValue(bool value)
    {
        switchIsActive = value;
    }
}
