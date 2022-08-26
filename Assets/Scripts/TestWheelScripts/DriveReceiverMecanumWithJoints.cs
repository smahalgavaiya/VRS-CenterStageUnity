using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class DriveReceiverMecanumWithJoints : DriveReceiverMecanum
{

    private Rigidbody rootRig;

    public float coefficientOfTranslation = .01f;
    float coefficientOfRotation = .05f;//was .5

    Vector3 movementDirection = Vector3.zero;
    float rotationDirection = 0;

    public GameObject rootBody;

    

    private void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        if (rootBody!=null)
        {
            rootRig = rootBody.GetComponent<Rigidbody>();
        }
        else { rootBody = gameObject; }
    }
    Vector3 CalculateDirection()
    {
        movementDirection = new Vector2(frontLeft.driveAmount.x - frontRight.driveAmount.x - backLeft.driveAmount.x + backRight.driveAmount.x,
            frontLeft.driveAmount.x + frontRight.driveAmount.x + backLeft.driveAmount.x + backRight.driveAmount.x);

        movementDirection = new Vector3(-movementDirection.x, 0, movementDirection.y);// TODO fix this

        return movementDirection;
    }

    float CalculateRotation()
    {
        rotationDirection = frontLeft.driveAmount.x + backLeft.driveAmount.x - frontRight.driveAmount.x - backRight.driveAmount.x;

        return rotationDirection;
    }

    private void Update()
    {
        //rigidbody.MovePosition((transform.TransformDirection(CalculateDirection()) * coefficientOfTranslation) + transform.position);//Tries to move up+down??
        //oddly enough the below rotates robot pretty well after bucking forward.
        rootRig.MovePosition((rootRig.transform.TransformDirection(CalculateDirection()) * coefficientOfTranslation) + rootRig.transform.position);
        //adding relative torque would only work if it was at each wheels position.
        //rootRig.MoveRotation(Quaternion.AngleAxis(CalculateRotation() * coefficientOfRotation, rootBody.transform.up) * rootBody.transform.rotation);
        if(!rootRig)
        {
            rigidbody.transform.Rotate(new Vector3(0, (CalculateRotation() * coefficientOfRotation),0 ));//WORKS
        }
        else { rootRig.transform.Rotate(new Vector3(0, (CalculateRotation() * coefficientOfRotation), 0));}
        //looks like he used moverotation because using moveposition+rotate would stop movement.
    }


}
