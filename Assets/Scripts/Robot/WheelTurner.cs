using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This class provides a visual indication of wheels turning when the robot moves
public class WheelTurner : MonoBehaviour
{
    [SerializeField]
    WheelCollider wheelCollider;

    public WheelCollider WheelCollider { get { return wheelCollider; } set { wheelCollider = value; } }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void FixedUpdate()
    {
        transform.Rotate(transform.right, wheelCollider.rpm * Time.fixedDeltaTime * 6);
    }
}
