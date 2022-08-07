using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdjacentJunctionDetector : MonoBehaviour
{
    public List<JunctionCapper> AdjacentJunctionCappers { get; set; }
    BoxCollider boxCollider;
    // Start is called before the first frame update
    void Start()
    {
        AdjacentJunctionCappers = new List<JunctionCapper>();
        boxCollider = GetComponent<BoxCollider>();
        CheckForAdjacentJunctionCappers();
    }

    private void CheckForAdjacentJunctionCappers()
    {
        Collider[] overlappingColliders = Physics.OverlapBox(transform.position, boxCollider.bounds.extents);

        for (int i = 0; i < overlappingColliders.Length; i++)
        {
            if (overlappingColliders[i].GetComponent<JunctionCapper>() != null)
            {
                AdjacentJunctionCappers.Add(overlappingColliders[i].GetComponentInChildren<JunctionCapper>());
            }
        }

        JunctionCapper theJunctionCapperOnThisJunction = transform.parent.GetComponentInChildren<JunctionCapper>();
        AdjacentJunctionCappers.Remove(theJunctionCapperOnThisJunction);
    }

    public void CheckAdjacentColor(CheckForCircuit checkForCircuit)
    {
        Stack<TeamColor> objectsOnJunction = transform.parent.GetComponentInChildren<JunctionCapper>().ObjectsOnJunction;

        if (objectsOnJunction.Count < 1)
            return;

        TeamColor thisJunctionCapColor = transform.parent.GetComponentInChildren<JunctionCapper>().ObjectsOnJunction.Peek();
        foreach (JunctionCapper junctionCapper in AdjacentJunctionCappers)
        {
            if (junctionCapper.ObjectsOnJunction.Count > 0 && 
                junctionCapper.ObjectsOnJunction.Peek() == thisJunctionCapColor)
            {
                if (CheckForTerminus(checkForCircuit))
                {
                    checkForCircuit.CircuitFound.boolValue = true;
                }

                junctionCapper.transform.parent.parent.GetComponentInChildren<AdjacentJunctionDetector>().CheckAdjacentColor(checkForCircuit);
            }
        }
    }

    bool CheckForTerminus(CheckForCircuit checkForCircuit)
    {
        bool terminusFound = false;

        for (int i = 0; i < checkForCircuit.TerminusJunctions.Length; i++)
        {
            if (checkForCircuit.TerminusJunctions[i] == this.gameObject)
                terminusFound = true;
        }

        return terminusFound;
    }
}
