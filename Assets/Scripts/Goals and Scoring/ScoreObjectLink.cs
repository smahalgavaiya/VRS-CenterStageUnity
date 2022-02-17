using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class ScoreObjectLink : MonoBehaviour
{
    public ScoringObjectLocation _ScoringObjectLocation { get; set; }
    public int Index { get; set; }
    public SpawnType _SpawnType { get; set; }

    // Update is called once per frame
    void Update()
    {
        if (_SpawnType == SpawnType.AtSpecificPoints || _SpawnType == SpawnType.RandomOverMultiplePoints)
        {
            _ScoringObjectLocation.pointPositions[Index] = transform.position;
        }
        else if (_SpawnType == SpawnType.RandomOverArea)
        {
            _ScoringObjectLocation.SpawnAreaCenter = transform.position;
            _ScoringObjectLocation.SpawnScale = transform.localScale;
        }
    }
}
