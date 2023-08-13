using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DisplayScoring : MonoBehaviour
{
    public ScoringManager scoring;
    private List<ScoreEntry> entries;
    private TextMeshProUGUI text;
    // Start is called before the first frame update
    private void OnEnable()
    {
        text = GetComponentInChildren<TextMeshProUGUI>();
        List<ScoreEntry> entries = scoring.GetScoreEntries();
        Debug.Log(entries);
    }

    private void Update()
    {
        text.text = scoring.ToString();
    }

}
