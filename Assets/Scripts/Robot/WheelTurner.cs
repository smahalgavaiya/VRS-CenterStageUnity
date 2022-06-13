using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This class provides a visual indication of wheels turning when the robot moves
public class WheelTurner : MonoBehaviour
{
    float moveDelta, wheelCircumfrence;
    private Vector3 previousPosition;

    // Start is called before the first frame update
    void Start()
    {
        previousPosition = transform.position;
        wheelCircumfrence = transform.lossyScale.y * Mathf.PI;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        moveDelta = Vector3.Distance(previousPosition, transform.position);
        Vector3 heading = transform.position - previousPosition;
        float angle = Vector3.Dot(heading.normalized, transform.parent.forward.normalized);

        previousPosition = transform.position;

        transform.Rotate(transform.right, angle * moveDelta / wheelCircumfrence * 360);
    }
}
