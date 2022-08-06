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

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
