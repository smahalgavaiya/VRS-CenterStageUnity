using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceTest : MonoBehaviour
{

    [SerializeField]
    protected Drive frontLeft, frontRight, backLeft, backRight;

    public Drive FrontLeft { set { frontLeft = value; } }
    public Drive FrontRight { set { frontRight = value; } }
    public Drive BackLeft { set { backLeft = value; } }
    public Drive BackRight { set { backRight = value; } }

    protected Rigidbody rigidbody;

    [SerializeField] float coefficientOfTranslation = .002f;
    float coefficientOfRotation = .5f;

    Vector3 movementDirection = Vector3.zero;
    float rotationDirection = 0;

    private void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
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
        if (rigidbody.velocity.magnitude < 1)
            rigidbody.AddForce(CalculateDirection() * coefficientOfTranslation);

        rigidbody.AddForce(-rigidbody.velocity * rigidbody.mass);


        //rigidbody.MoveRotation(Quaternion.AngleAxis(CalculateRotation() * coefficientOfRotation, transform.up) * transform.rotation);

        //transform.Translate(CalculateDirection() * coefficientOfMotion, Space.Self); 
        //transform.Rotate(new Vector3(0, 0, CalculateRotation() * coefficientOfMotion));
    }
}
