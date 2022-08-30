using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;

public enum FieldGameMode
{
    Autonomous,
    Teleop
}
public class FieldManager : MonoBehaviour
{
    public FieldGameMode mode;

    public RoundIndex autonomous,teleop;

    public GameTimeManager gameTimeManager;

    private static FieldManager _instance;
    public static FieldManager instance
    {
        get {return _instance;}
    }

    [DllImport("__Internal")]
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
    public void Start()
    {
#if !UNITY_EDITOR
        if (vrs_messenger.instance == null)
            mode = GameMode.Teleop;
        else
            mode = vrs_messenger.instance.GetPlaymode();
#endif
        gameTimeManager.roundIndex = GetRoundIndex();
        GameTimeReceiver timeReceiver = FindObjectOfType<GameTimeReceiver>();
        //timeReceiver.roundIndex = GetRoundIndex();

        
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
    public RoundIndex GetRoundIndex()
    {
        if(mode == FieldGameMode.Autonomous)
            return autonomous;
        else if(mode == FieldGameMode.Teleop)
            return teleop;
        else
            return null;
    }

}
