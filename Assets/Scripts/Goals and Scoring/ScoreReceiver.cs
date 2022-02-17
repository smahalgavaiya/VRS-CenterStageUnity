using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreReceiver : MonoBehaviour
{
    public ScoreTracker scoreTracker;
    string teamName;
    Text text;

    // Start is called before the first frame update
    void Start()
    {
        if (scoreTracker.name.Contains("Red") || scoreTracker.name.Contains("red"))
        {
            teamName = "Red";
        } else
        {
            teamName = "Blue";
        }

        text = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        text.text = teamName + ": " + scoreTracker.Score;
    }
}
