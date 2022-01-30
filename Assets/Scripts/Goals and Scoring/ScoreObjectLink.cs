using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class ScoreObjectLink : MonoBehaviour
{
    public ScoringObjectLocation ScoringObject_ { get; set; }
    public int Index { get; set; }
    public SpawnType SpawnType_ { get; set; }

    // Update is called once per frame
    void Update()
    {
        if (SpawnType_ == SpawnType.AtSpecificPoints)
        {
            ScoringObject_.pointPositions[Index] = transform.position;
        }
    }
}
