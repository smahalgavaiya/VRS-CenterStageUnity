using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cone : MonoBehaviour
{
    [SerializeField] GameObject coneMeshObject;

    public GameObject ConeMeshObject { get => coneMeshObject; }

    [SerializeField] GameObject junctionSliderObject;
    public GameObject JunctionSliderObject { get => junctionSliderObject; }

    [SerializeField] GameObject coneBaseForStacking;
    public GameObject ConeBaseForStacking { get => coneBaseForStacking; }

    public GameObject ConeCollider;

    public void MakeScorable(bool scorable = true)
    {
        Rigidbody rig = GetComponent<Rigidbody>();
        GetComponent<Cone>().ConeBaseForStacking.GetComponentInChildren<Collider>().enabled = !scorable;//turn off stack collider.
        rig.isKinematic = !scorable;
        ConeCollider.GetComponent<Collider>().enabled = scorable;
        rig.ResetInertiaTensor();


        //these changes are all because unity decided to have some crazy bug where it would set the velocity sky high and
        //cause the cone to bust through the floor.
        rig.velocity = new Vector3(0, 0, 0);
        rig.useGravity = false;

        rig.isKinematic = true;
        Invoke("resetGrav",0.01f);
    }

    private void resetGrav()
    {
        Rigidbody rig = GetComponent<Rigidbody>();
        rig.useGravity = true;
        rig.isKinematic = false;
    }

}
