using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class DriveReceiverForMecanumWheel : DriveReceiver
{
    public float power = 0;
    public bool runWithEncoder = true;
    public float powerMultiplier = 40000;

    bool mecanum = true;

    Vector3 wheelPos;
    Quaternion wheelRot;
    Rigidbody rb;
    Transform slipDirection;
    Transform wheelMesh;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        wheelPos = transform.position;
        wheelRot = transform.rotation;
        rb = GetComponent<Rigidbody>();
        slipDirection = transform.GetChild(1);
        wheelMesh = transform.GetChild(2);
    }

    public void setPower(float power)
    {
        power = Mathf.Clamp(power, -1, 1);
        this.power = power;
    }

    private void Update()
    {
        Vector3 veloc = rb.velocity;
        float wheelVeloc = Vector3.Dot(veloc, slipDirection.forward);
        //Friction
        if (wheelVeloc > .01 && power <= 0)
            rb.AddForce(-slipDirection.forward * Mathf.Min(powerMultiplier, wheelVeloc * powerMultiplier) * Time.deltaTime * (runWithEncoder ? 2 : 1));
        else if (wheelVeloc < -.01 && power >= 0)
            rb.AddForce(slipDirection.forward * Mathf.Min(powerMultiplier, -wheelVeloc * powerMultiplier) * Time.deltaTime * (runWithEncoder ? 2 : 1));
        //Wheel Power
        if (Mathf.Abs(wheelVeloc) < 6) //RPM
            rb.AddForce(slipDirection.forward * power * powerMultiplier * Time.deltaTime); //Torque
                                                                                 //Wheel Graphics
                                                                                 //if (UIBehavior.currScene != UIBehavior.sceneState.Config)
        //wheelMesh.localRotation *= Quaternion.Euler(0, -wheelVeloc / (mecanum ? 1.4142f : 1), 0);
        //else
        wheelMesh.localRotation *= Quaternion.Euler(0, -power * 1000 * Time.deltaTime, 0);
    }

    //Used in Config
    public void resetWheel()
    {
        transform.position = wheelPos;
        transform.rotation = wheelRot;
        //wheelMesh.localRotation = Quaternion.identity;
    }
}
