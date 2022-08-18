using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;

public enum GameMode
{
    None,
    Autonomous,
    Teleop
}
public class FieldManager : MonoBehaviour
{
    public GameMode mode;

    public RoundIndex autonomous,teleop;

    public PowerPlayGameTimeManager gameTimeManager;

    private static FieldManager _instance;
    public static FieldManager instance
    {
        get {return _instance;}
    }

    [DllImport("_Internal")]
    private static extern void updateSignalSensor(int signal);
    private bool hasCheckedSignalSensor = false;
    void Awake()
    {
        if(_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }
    void Start()
    {
        mode = vrs_messenger.instance.GetPlaymode();
        if(mode == GameMode.Autonomous)
            gameTimeManager.roundIndex = autonomous;
        else if(mode == GameMode.Teleop)
            gameTimeManager.roundIndex = teleop;

        
    }
    void Update()
    {
        if(!hasCheckedSignalSensor)
            checkSignalSensor();
    }
    void checkSignalSensor()
    {
        int signalType = 0;
        GameObject signalConeParent = FindObjectOfType<SignalRandomizer>().gameObject;
        foreach(Transform child in signalConeParent.transform)
        {
            signalType++;
            if(child.gameObject.activeInHierarchy)
                break;
        }
        #if UNITY_WEBGL && !UNITY_EDITOR
        try{updateSignalSensor(signalType);}
        catch{}
        #endif

        hasCheckedSignalSensor = true;
    }

}
