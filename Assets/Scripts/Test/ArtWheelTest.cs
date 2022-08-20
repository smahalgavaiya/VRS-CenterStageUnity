using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ArtWheelTest : MonoBehaviour
{
    private float motionFB;
    private float motionRL;

    [SerializeField] List<GameObject> wheels;

    [SerializeField] float coefficient;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnMovement(InputValue value)
    {
        motionFB = value.Get<Vector2>().y;
        motionRL = value.Get<Vector2>().x;

    }
    private void FixedUpdate()
    {
        foreach(GameObject wheel in wheels)
        {
            ArticulationBody body = wheel.GetComponent<ArticulationBody>();
            body.AddTorque(wheel.transform.parent.right * motionFB * coefficient);
            body.AddTorque(wheel.transform.parent.forward * -motionRL * coefficient);

        }
    }
}
