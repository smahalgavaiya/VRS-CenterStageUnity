using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class WheelTest : MonoBehaviour
{
    [SerializeField] GameObject wheelModelsFBParent, wheelModelsRLParent, wheelCollidersFBParent, wheelCollidersRLParent;

    List<WheelCollider> wheelCollidersRL, wheelCollidersFB;

    List<GameObject> wheelModelsFB, wheelModelsRL;

    float motionFB = 0, motionRL;

    [SerializeField] float motionSpeed = 0;

    // Start is called before the first frame update
    void Start()
    {
        wheelCollidersFB = new List<WheelCollider>();
        wheelCollidersRL = new List<WheelCollider>();

        wheelModelsFB = new List<GameObject>();
        wheelModelsRL = new List<GameObject>();

        for (int i = 0; i < wheelCollidersFBParent.transform.childCount; i++)
        {
            wheelCollidersFB.Add(wheelCollidersFBParent.transform.GetChild(i).GetComponent<WheelCollider>());
        }
        for (int i = 0; i < wheelCollidersRLParent.transform.childCount; i++)
        {
            wheelCollidersRL.Add(wheelCollidersRLParent.transform.GetChild(i).GetComponent<WheelCollider>());
        }

        for (int i = 0; i < wheelModelsFBParent.transform.childCount; i++)
        {
            wheelModelsFB.Add(wheelModelsFBParent.transform.GetChild(i).gameObject);
        }
        for (int i = 0; i < wheelModelsRLParent.transform.childCount; i++)
        {
            wheelModelsRL.Add(wheelModelsRLParent.transform.GetChild(i).gameObject);
        }

        foreach (WheelCollider wheelCollider in wheelCollidersRL)
        {
            wheelCollider.steerAngle = 90;
        }
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
        foreach (WheelCollider wheelCollider in wheelCollidersFB)
        {
            wheelCollider.motorTorque = (motionFB + motionRL) * motionSpeed;
            wheelCollider.steerAngle = Mathf.Lerp(0, 90, motionRL) ;

            if (motionFB == 0)
            {
                wheelCollider.brakeTorque = 1200;
            }
            else wheelCollider.brakeTorque = 0;
        }
        //foreach (WheelCollider wheelCollider in wheelCollidersRL)
        //{
        //    wheelCollider.motorTorque = motionRL * motionSpeed;

        //    if (motionRL == 0)
        //    {
        //        wheelCollider.brakeTorque = 1200;
        //    }
        //    else wheelCollider.brakeTorque = 0;
        //}
        for(int i = 0; i < wheelModelsFB.Count; i++)
        {
            Vector3 wheelPos;
            Quaternion wheelRot;

            wheelCollidersFB[i].GetWorldPose(out wheelPos, out wheelRot);

            wheelModelsFB[i].transform.position = wheelPos;
            wheelModelsFB[i].transform.rotation = wheelRot;
        }

        //for(int i = 0; i < wheelModelsRL.Count; i++)
        //{
        //    Vector3 wheelPos;
        //    Quaternion wheelRot;

        //    wheelCollidersRL[i].GetWorldPose(out wheelPos, out wheelRot);

        //    wheelModelsRL[i].transform.position = wheelPos;
        //    wheelModelsRL[i].transform.rotation = wheelRot;
        //}
        
    }
}
