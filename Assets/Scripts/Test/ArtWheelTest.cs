using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ArtWheelTest : MonoBehaviour
{
    private float motionFB, motionRL, rotationRL;

    [SerializeField] GameObject robotBody, frontLeftWheel, 
        backLeftWheel, frontRightWheel, backRightWheel;

    ArticulationBody frontLeftArtBod, backLeftArtBod, frontRightArtBod, backRightArtBod;
    
    List<GameObject> wheels;
    List<ArticulationBody> wheelArticulationBodies;

    Vector3 robotRightVector, robotForwardVector;

    [SerializeField] float coefficient;

    // Start is called before the first frame update
    void Start()
    {
        SetRobotVectors();

        wheels = new List<GameObject>();
        wheelArticulationBodies = new List<ArticulationBody>();

        wheels.Add(frontRightWheel);
        wheels.Add(frontLeftWheel);
        wheels.Add(backLeftWheel);
        wheels.Add(backRightWheel);

        foreach (GameObject wheel in wheels)
        {
            wheelArticulationBodies.Add(wheel.GetComponent<ArticulationBody>());
        }

        frontRightArtBod = frontRightWheel.GetComponent<ArticulationBody>();
        frontLeftArtBod = frontLeftWheel.GetComponent<ArticulationBody>();
        backRightArtBod = backRightWheel.GetComponent<ArticulationBody>();
        backLeftArtBod = backLeftWheel.GetComponent<ArticulationBody>();
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
    public void OnRotation(InputValue value)
    {
        rotationRL = value.Get<float>();
    }
    private void FixedUpdate()
    {
        foreach(ArticulationBody wheelArtBod in wheelArticulationBodies)
        {
            wheelArtBod.AddTorque(robotRightVector * motionFB * coefficient);
            wheelArtBod.AddTorque(robotForwardVector * -motionRL * coefficient);
        }

        RotateDirectionally(frontLeftArtBod, backLeftArtBod, 1);
        RotateDirectionally(frontRightArtBod, backRightArtBod, -1);
    }

    private void RotateDirectionally(ArticulationBody artBodOne, 
        ArticulationBody artBodTwo, 
        int direction)
    {
        artBodOne.AddTorque(robotBody.transform.right.normalized * rotationRL * coefficient * direction);
        artBodTwo.AddTorque(robotBody.transform.right.normalized * rotationRL * coefficient * direction);

        SetRobotVectors();
    }

    private void SetRobotVectors()
    {
        robotForwardVector = robotBody.transform.forward.normalized;
        robotRightVector = robotBody.transform.right.normalized;
    }
}
