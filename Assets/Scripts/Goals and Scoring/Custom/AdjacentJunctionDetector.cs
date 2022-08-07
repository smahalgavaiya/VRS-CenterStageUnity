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

}
