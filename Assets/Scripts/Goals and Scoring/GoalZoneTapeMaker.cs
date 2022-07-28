using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This class draws tape on the floor automatically around Goal Zones
[ExecuteInEditMode]
public class GoalZoneTapeMaker : TapeMaker
{

    // Set tape color depending on team
    public void SetTapeColor(TeamColor scoreZone)
    {
        if (tapeSides[0] == null)
            GetTapeSides();

        foreach (GameObject tapeSide in tapeSides)
        {
            switch (scoreZone)
            {
                case TeamColor.Blue:
                    tapeSide.GetComponent<Renderer>().material = materialIndex.blueTapeMaterial;
                    break;
                case TeamColor.Red:
                    tapeSide.GetComponent<Renderer>().material = materialIndex.redTapeMaterial;
                    break;
            }
        }
    }
}
