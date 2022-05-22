using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeutralHubTiltSensor : MonoBehaviour
{
    [SerializeField]
    LayerMask layerMask;

    [SerializeField]
    GameObject blueTiltSensor, redTiltSensor;

    RaycastHit blueHitFloor, redHitFloor;

    float redDistanceToFloor, blueDistanceToFloor;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Physics.Raycast(blueTiltSensor.transform.position, Vector3.down, out blueHitFloor, 20, layerMask);
        Physics.Raycast(redTiltSensor.transform.position, Vector3.down, out redHitFloor, 20, layerMask);

        redDistanceToFloor = Vector3.Distance(redTiltSensor.transform.position, redHitFloor.point);
        blueDistanceToFloor = Vector3.Distance(blueTiltSensor.transform.position, blueHitFloor.point);
    }
}
