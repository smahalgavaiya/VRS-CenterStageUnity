using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

public class InputActionManager : MonoBehaviour
{
    public UnityEvent pickUpObject;

    public Drive forwardBack, leftRight;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void OnMovement(InputValue value)
    {
        forwardBack.SendValue(value.Get<Vector2>().y);
        leftRight.SendValue(value.Get<Vector2>().x);
    }

    public void OnPickUpObject(InputValue value)
    {
        if (value.isPressed)
            pickUpObject.Invoke();
    }


    // Update is called once per frame
    void Update()
    {
        
        
    }
}
