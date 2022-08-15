using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReleaseSubstationCone : MonoBehaviour
{
    List<GameObject> conePositions = new List<GameObject>();
    [SerializeField] GameObject conePositionsParent;

    List<GameObject> cones;

    int numberOfConesReleased = 0;

    // Start is called before the first frame update
    void Start()
    {
        conePositions = new List<GameObject>(); 

        cones = new List<GameObject>();

        foreach(Cone cone in GetComponentsInChildren<Cone>())
        {
            cones.Add(cone.gameObject);
        }

        for(int i = 0; i < conePositionsParent.transform.childCount; i++)
        {
            conePositions.Add(conePositionsParent.transform.GetChild(i).gameObject);
        }
    }

    public void ReleaseNewCone()
    {
        // We can only fit a certain number of cones in the substation

        if (numberOfConesReleased > cones.Count - 1)
            return;

        for (int i = 0; i < conePositions.Count; i++)
        {
            if (conePositions[i].GetComponent<ConeOccupancyChecker>().IsEmptyNoCone)
            {
                GameObject coneToRelease = cones[numberOfConesReleased];
                coneToRelease.GetComponentInParent<TopConeFreedomChecker>().ReleaseConeManually();

                coneToRelease.GetComponent<Rigidbody>().isKinematic = false;

                coneToRelease.transform.position = conePositions[i].transform.position;
                numberOfConesReleased++;
                break;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
