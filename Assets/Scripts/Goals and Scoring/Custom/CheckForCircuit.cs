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
    public TeamColor CircuitColor { get => circuitColor;}

    bool circuitPreviouslyFound;

    public List<JunctionCapper> JunctionCappersChecked { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        CircuitFound.boolValue = false;
        JunctionCappersChecked = new List<JunctionCapper>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CheckCircuit()
    {
        CircuitFound.boolValue = false;
        JunctionCappersChecked.Clear();

        for (int i = 0; i < startJunctions.Length; i++)
        {
            AdjacentJunctionDetector adjacentJunctionDetector = startJunctions[i].GetComponentInChildren<AdjacentJunctionDetector>();
            adjacentJunctionDetector.CheckAdjacentColor(this);
        }

        if (CircuitFound.boolValue && !circuitPreviouslyFound)
            scoreTracker.AddOrSubtractScore(20);

        else if (!CircuitFound.boolValue && circuitPreviouslyFound)
            scoreTracker.AddOrSubtractScore(-20);

        circuitPreviouslyFound = CircuitFound.boolValue;
    }
    public void CircuitIsFound()
    {
    }
}
