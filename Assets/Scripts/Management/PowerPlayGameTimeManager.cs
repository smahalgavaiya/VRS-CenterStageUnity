using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerPlayGameTimeManager : GameTimeManager
{
    bool isCountingTime = false;

    bool hasPlayed = false;

    private int timeInRoundsPrior = 0;

    void Update()
    {

        if (!isCountingTime)
            return;

        gameTime.globalInt = (int)Time.realtimeSinceStartup - timeInRoundsPrior;

        // Progress to the next round
        if (gameTime.globalInt > roundIndex.rounds[roundIndex.currentRound].roundLength)
        {
            timeInRoundsPrior += roundIndex.rounds[roundIndex.currentRound].roundLength;
            roundIndex.currentRound++;
            if (roundIndex.currentRound > roundIndex.rounds.Count - 1)
                isCountingTime = false;
        }
        
    }
    public void StartGame()
    {
        if(!hasPlayed)
        {
            hasPlayed = true;
            ResetGame();
        }
        isCountingTime = true;
    }
    public void StopGame()
    {
        isCountingTime = false;
    }
    public void ResetGame()
    {
        gameStarts.Invoke();
        timeInRoundsPrior = (int)Time.realtimeSinceStartup;
    }

}
