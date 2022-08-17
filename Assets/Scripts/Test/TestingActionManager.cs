using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestingActionManager : MonoBehaviour
{
    public EncoderActionManager manager;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        manager.SetFrontLeft(1);
        manager.SetFrontRight(1);
        manager.SetBackLeft(1);
        manager.SetBackRight(1);
    }
}
