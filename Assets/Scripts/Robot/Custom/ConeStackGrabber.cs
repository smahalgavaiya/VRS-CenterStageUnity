using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConeStackGrabber : MonoBehaviour
{
    [SerializeField] Drive grabberDrive;
    bool isOnConeStack;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<ConeStackRBManager>() != null)
        {
            isOnConeStack = true;
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.GetComponent<ConeStackRBManager>() != null)
        {
            if (grabberDrive.driveAmount.magnitude > .01f && isOnConeStack)
            {
                other.GetComponent<ConeDispenser>().DispenseCone();
                GameObject newCone = 
                    Instantiate(other.GetComponent<ConeStackRBManager>().PhysicalCones.transform.GetChild(0).gameObject);
                newCone.transform.position = this.transform.position;
                GetComponent<ObjectChecker>().CanPickUp = true;
                GetComponent<ObjectChecker>().ObjectInTrigger = newCone;
                GetComponent<ObjectGrabber>().PickUpOrPutDownObject();
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<ConeStackRBManager>() != null)
        {
            isOnConeStack = false;
        }
    }
}
