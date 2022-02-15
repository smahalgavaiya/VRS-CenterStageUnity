using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameTimeManager : MonoBehaviour
{
    public UnityEvent gameStarts;
    public GlobalInt gameTime;
    public RoundIndex roundIndex;

    int timeInPriorRounds = 0;

    bool countingTime = true;

    // Start is called before the first frame update
    void Start()
    {
        gameStarts.Invoke();
    }

    // Update is called once per frame
    void Update()
    {
        if (!countingTime)
            return;

        gameTime.globalInt = (int)Time.realtimeSinceStartup;

        // Progress to the next round
        if (gameTime.globalInt - timeInPriorRounds > roundIndex.rounds[roundIndex.currentRound].roundLength)
        {
            timeInPriorRounds += roundIndex.rounds[roundIndex.currentRound].roundLength;
            roundIndex.currentRound++;
            if (roundIndex.currentRound > roundIndex.rounds.Count - 1)
                countingTime = false;
        }
        
    }
}
