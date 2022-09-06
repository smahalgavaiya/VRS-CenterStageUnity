using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;
using System;

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

    public Session autonomous, TeleOpMid, TeleOpEnd, FreePlay;

    public GameTimeManager gameTimeManager;

    private static FieldManager _instance;
    public static FieldManager instance
    {
        get { return _instance; }
    }

    [SerializeField] GlobalInt currentSession;

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
        Session[] allSessions = new Session[4] { autonomous, TeleOpMid, TeleOpEnd, FreePlay };
        bool[] playSessions = GetActiveSessions();
        for (int i = 0; i < allSessions.Length; i++)
        {
            allSessions[i].playThisSession = playSessions[i];
        }

    }
    public bool[] GetActiveSessions()
    {
        switch(mode)
        {
            case GameMode.Autonomous:
                return new bool[4] {true, false, false, false};
            case GameMode.Teleop:
                return new bool[4] {false, true, true, false};
            case GameMode.Fullgame:
                return new bool[4] {true, true, true, false};
            case GameMode.Freeplay:
                return new bool[4] {false, false, false, true};
        }
        return new bool[4] {false, false, false, false};
    }

}
