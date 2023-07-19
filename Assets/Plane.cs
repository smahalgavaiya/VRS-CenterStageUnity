using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plane : MonoBehaviour
{
    private Rigidbody rig;
    bool released = false;
    public float power = 50;
    // Start is called before the first frame update
    void Start()
    {
        rig = GetComponentInChildren<Rigidbody>();

    }

    public void Release(float force)
    {
        if(!released)
        {
            transform.parent = null;
            rig.isKinematic = false;
            rig.velocity = -transform.up * (force * power);
            //rig.AddRelativeForce(-transform.forward* (force * 100));
            released = true;
        }


    }
}
