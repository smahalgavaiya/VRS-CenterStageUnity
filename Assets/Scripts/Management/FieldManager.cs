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

    public Session[] autonomousSessions,teleopSessions,fullgameSessions,freeplaySessions;
    public Round[] autonomousRound, teleOpRound, fullgameRound,freeplayRound;

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
        var (sessions, rounds) = GetRoundIndex();
        gameTimeManager.sessions = sessions;
        gameTimeManager.rounds = rounds;

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
        var (sessions, rounds) = GetRoundIndex();
        gameTimeManager.sessions = sessions;
        gameTimeManager.rounds = rounds;
    }
    public (Session[], Round[]) GetRoundIndex()
    {
        switch(mode)
        {
            case GameMode.Autonomous:
                return (autonomousSessions, autonomousRound);
            case GameMode.Teleop:
                return (teleopSessions, teleOpRound);
            case GameMode.Fullgame:
                return (fullgameSessions, fullgameRound);
            case GameMode.Freeplay:
                return (freeplaySessions, freeplayRound);
        }
        return (null,null);
    }

}
