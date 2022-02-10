using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Goals/Score Tracker")]
public class ScoreTracker : ScriptableObject
{
    public int Score { get; set; } = 0;

    public void AddOrSubtractScore(int amountToChange)
    {
        Score += amountToChange;
    }

    public void ResetScore()
    {
        Score = 0;
    }

}
