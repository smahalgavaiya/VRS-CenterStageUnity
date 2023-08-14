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
        gameTime = FindFirstObjectByType<GameTimeManager>();
        FindFirstObjectByType<PowerPlayFieldManager>().SetGameMode(gameMode);
        PowerPlayFieldManager.instance.SetGameMode(gameMode);
        GameTimeManager.instance.SetUpTimer();
    }

    public void ReleaseObj(string team)
    {
        //gameTime.SetUpTimer();
    }

    public void Play()
    {
        GameTimeManager.instance.Play();
    }

    public void Stop()
    {
        GameTimeManager.instance.Stop();
    }

    public void Reset()
    {
        GameTimeManager.instance.ResetTime();
    }

}
