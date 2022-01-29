using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Goals/Score Tracker")]
public class ScoreTracker : ScriptableObject
{
    public int Score { get; set; } = 0;

    public void IncreaseScore()
    {
        Score++;
    }
    public void IncreaseScore(int amountToChange)
    {
        Score += amountToChange;
    }

    public void DecreaseScore()
    {
        Score--;
    }
    public void DecreaseScore(int amountToChange)
    {
        Score -= amountToChange;
    }
}
