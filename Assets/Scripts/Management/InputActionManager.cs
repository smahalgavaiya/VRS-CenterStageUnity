using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

public class InputActionManager : MonoBehaviour
{
    public UnityEvent pickUpObject;

    public Drive forwardBack, leftRight, redCarousel;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void OnMovement(InputValue value)
    {
        Debug.Log(value.Get<Vector2>().x);
        redCarousel.SendValue(new Vector3(0, value.Get<Vector2>().x * 2, 0));
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
