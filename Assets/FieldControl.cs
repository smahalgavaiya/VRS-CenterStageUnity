using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReleaseDetails
{
    public string name;
    public Team team;
}

public class FieldControl : MonoBehaviour
{
    //For UI to interface with field managers/timers etc
    private GameTimeManager gameTime;
    private void Start()
    {
        gameTime = FindFirstObjectByType<GameTimeManager>();
        RoundReceiver round = FindFirstObjectByType<RoundReceiver>();
        GameTimeManager.instance.onPauseEv += round.SetPausedText;
        GameTimeManager.instance.onResumeEv += round.SetRoundText;
    }
    public void StartTimer()
    {
        gameTime.Play();
    }

    public void SetMode(int gameMode)
    {
        PowerPlayFieldManager.instance.SetGameMode(gameMode);
        gameTime.SetUpTimer();
    }

    public void ReleaseObj(string team)
    {
        gameTime.SetUpTimer();
    }

    public void Play()
    {
        gameTime.Play();
    }

    public void Stop()
    {
        gameTime.Stop();
    }

    public void Reset()
    {
        gameTime.ResetTime();
    }
}
