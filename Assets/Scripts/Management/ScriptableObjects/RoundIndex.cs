using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Index/Round Index")]
public class RoundIndex : ScriptableObject
{
    public List<Round> rounds;
}

[System.Serializable]
public class Round
{
    public string roundName;
    [Tooltip("Round length in seconds")]
    public int roundLength;
}
