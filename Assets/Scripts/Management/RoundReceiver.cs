using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoundReceiver : MonoBehaviour
{
    public RoundIndex roundIndex;
    Text text;

    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        if (roundIndex.currentRound > roundIndex.rounds.Count - 1)
            text.text = "Game Over";
        else
            text.text = "Round: " + (roundIndex.currentRound + 1);
    }
}
