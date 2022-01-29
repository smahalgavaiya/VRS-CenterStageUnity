using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Index/Score Index")]
public class ScoreIndex : ScriptableObject
{
    public ScoreTracker redScoreTracker;
    public ScoreTracker blueScoreTracker;
}
