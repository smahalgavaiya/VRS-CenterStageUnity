using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckForCircuit : MonoBehaviour
{
    [SerializeField] GameObject[] startJunctions;
    [SerializeField] GameObject[] terminusJunctions;

    [SerializeField] TeamColor circuitColor;
    [SerializeField] ScoreTracker scoreTracker;

    public GameObject[] TerminusJunctions { get => terminusJunctions; }

    [SerializeField] GlobalBool circuitFound;

    public GlobalBool CircuitFound { get => circuitFound;}

    bool circuitPreviouslyFound;

    // Start is called before the first frame update
    void Start()
    {
        CircuitFound.boolValue = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CheckCircuit()
    {
        CircuitFound.boolValue = false;

        for (int i = 0; i < startJunctions.Length; i++)
        {
            if (CircuitFound.boolValue)
            {
                CircuitIsFound();
                break;
            }
            AdjacentJunctionDetector adjacentJunctionDetector = startJunctions[i].GetComponentInChildren<AdjacentJunctionDetector>();
            adjacentJunctionDetector.CheckAdjacentColor(this);
        }

        if (circuitPreviouslyFound != CircuitFound.boolValue)
            scoreTracker.AddOrSubtractScore(-10);

        circuitPreviouslyFound = CircuitFound.boolValue;
    }
    public void CircuitIsFound()
    {
        if (circuitPreviouslyFound != CircuitFound.boolValue)
            scoreTracker.AddOrSubtractScore(10);

        circuitPreviouslyFound = CircuitFound.boolValue;
    }
}
