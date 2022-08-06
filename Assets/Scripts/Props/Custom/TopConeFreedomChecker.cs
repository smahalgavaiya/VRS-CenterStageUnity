using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Is the top cone on the stack free from the stack? (e.g. a robot picked it up successfully?)
public class TopConeFreedomChecker : MonoBehaviour
{
    float coneHeight, coneWidth;

    GameObject topCone, secondCone;
    Vector2 topConeStartXZ;

    ConeStacker coneStacker;

    Rigidbody topConeRB, secondConeRB;
    RigidbodyConstraints starterConstraints, freeConstraints;

    // Start is called before the first frame update
    void Start()
    {
        coneStacker = GetComponent<ConeStacker>();

        SetNewTopAndSecondCone();

        coneHeight = topCone.GetComponent<Cone>().ConeMeshObject.GetComponent<MeshFilter>().mesh.bounds.size.y;
        coneWidth = topCone.GetComponent<Cone>().ConeMeshObject.GetComponent<MeshFilter>().mesh.bounds.size.x;

        // Adjust the size for the scale factor of the mesh
        coneHeight = coneHeight / 100;
        coneWidth = coneHeight / 100;

        freeConstraints = new RigidbodyConstraints();
        freeConstraints = RigidbodyConstraints.None;

        starterConstraints = topConeRB.constraints;

        topConeStartXZ = new Vector2(topCone.transform.position.x, topCone.transform.position.z);
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(secondCone.transform.position, topCone.transform.position) > coneHeight)
        {
            UnlockConstraintsTopCone();
        }
        else { LockConstraintsTopCone(); }

        if (GetXZDistanceOfTopConeFromConeStack() > coneWidth / 6f)
        {
            coneStacker.ReleaseTopCone();
            SetNewTopAndSecondCone();
        }
    }

    private void SetNewTopAndSecondCone()
    {
        topCone = coneStacker.TopCone;
        secondCone = coneStacker.SecondCone;

        topConeRB = topCone.GetComponent<Rigidbody>();
        topConeRB.isKinematic = false;

        topCone.GetComponent<Cone>().JunctionSliderObject.SetActive(true);
        topCone.GetComponent<Cone>().ConeMeshObject.GetComponent<MeshCollider>().enabled = true;

        secondConeRB = secondCone.GetComponent<Rigidbody>();
    }

    private void LockConstraintsTopCone()
    {
        topConeRB.constraints = starterConstraints;
        topCone.transform.position = new Vector3(topConeStartXZ.x, topCone.transform.position.y, topConeStartXZ.y);
    }

    void UnlockConstraintsTopCone()
    {
        topConeRB.constraints = freeConstraints;
    }

    float GetXZDistanceOfTopConeFromConeStack()
    {
        float XZDistance = Vector2.Distance(new Vector2(topCone.transform.position.x, topCone.transform.position.z),
            new Vector2(secondCone.transform.position.x, secondCone.transform.position.z));
        return XZDistance;
    }
}
