using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

public class InputActionManager : MonoBehaviour
{

    public UnityEvent testEvent, endTestEvent;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void OnTestAction(InputValue value)
    {
        if (value.isPressed)
            testEvent.Invoke();
        else
            endTestEvent.Invoke();
    }

    // Update is called once per frame
    void Update()
    {
        
        
    }
}
