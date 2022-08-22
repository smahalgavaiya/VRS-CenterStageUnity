using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnockedOverConeStackPenalizer : MonoBehaviour
{
    List<CheckLastTouchedColor> lastTouchedColorList;
    TeamColor coneStackColor;
    [SerializeField] ScoreTracker blueScoreTracker;
    [SerializeField] ScoreTracker redScoreTracker;

    [SerializeField] GlobalInt penalty;

    [SerializeField] int numberOfPenalties;

    // Start is called before the first frame update
    void Start()
    {
        lastTouchedColorList = new List<CheckLastTouchedColor>();

        foreach(CheckLastTouchedColor color in GetComponentsInChildren<CheckLastTouchedColor>())
        {
            lastTouchedColorList.Add(color);
        }

        coneStackColor = GetComponent<ConeStackColorSwitcher>().TeamColor_;
    }

    public void PenalizeOrNot()
    {
        foreach(CheckLastTouchedColor c in lastTouchedColorList)
        {
            if (c.LastTouchedColor != coneStackColor)
            { 
                switch (c.LastTouchedColor)
                {
                    case TeamColor.Blue:
                        blueScoreTracker.AddOrSubtractScore(-penalty.globalInt * numberOfPenalties);
                        break;
                    case TeamColor.Red:
                        redScoreTracker.AddOrSubtractScore(-penalty.globalInt * numberOfPenalties);
                        break;
                }
                break;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
