using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreOutput : MonoBehaviour
{
    public void SendScore()
    {
        Debug.Log("Sending score, stop being an asshole.");
        vrs_messenger.sendScore();

    }
}
