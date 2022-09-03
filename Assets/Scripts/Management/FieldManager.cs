using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;

public enum GameMode
{
    Autonomous=0,
    Teleop=1,
    Fullgame=2,
    Freeplay=3
}
public class FieldManager : MonoBehaviour
{
    public static TeamColor botColor;

    public GameMode mode;

    public Session autonomous,teleop,fullgame,freeplay;

    public GameTimeManager gameTimeManager;

    private static FieldManager _instance;
    public static FieldManager instance
    {
        get { return _instance; }
    }

    [SerializeField] GlobalInt currentRound;

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
        gameTimeManager.CurrentSession = GetRoundIndex();
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

    public void SetGameMode(int mode)
    {
        this.mode = (GameMode)mode;
        gameTimeManager.CurrentSession = GetRoundIndex();
    }
    public Session GetRoundIndex()
    {
        switch(mode)
        {
            case GameMode.Autonomous:
                return autonomous;
            case GameMode.Teleop:
                return teleop;
            case GameMode.Fullgame:
                return fullgame;
            case GameMode.Freeplay:
                return freeplay;
        }
        return null;
    }

}
