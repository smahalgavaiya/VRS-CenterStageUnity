using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class ScoreTarget : MonoBehaviour
{
    public RoundIndex roundIndex;

    [SerializeField]
    public List<int> roundScores;


    public ScoringObject[] scoringObjects;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

