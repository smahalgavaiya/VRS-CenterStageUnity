using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Turns off individual cone RBs until the stack is knocked over
public class ConeStackRBManager : MonoBehaviour
{
    [SerializeField] GameObject physicalCones;
    [SerializeField] GameObject dummyCones;

    public GameObject PhysicalCones { get => physicalCones; }

    // Start is called before the first frame update
    void Start()
    {
        PhysicalCones.SetActive(false);
    }

    public void SwitchOnPhysicalCones()
    {
        PhysicalCones.SetActive(true);
        dummyCones.SetActive(false);
    }    

    // Update is called once per frame
    void Update()
    {
        
    }

}
