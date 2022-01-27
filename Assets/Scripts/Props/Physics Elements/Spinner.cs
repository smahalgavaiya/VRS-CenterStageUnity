using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(SpinnerHelper))]
public class Spinner : MonoBehaviour
{
    Rigidbody spinnerRigidBody;
    public Vector3 rotationSpeed;
    // Start is called before the first frame update
    void Start()
    {
        spinnerRigidBody = GetComponent<Rigidbody>();
        spinnerRigidBody.isKinematic = true;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Quaternion deltaRotation = Quaternion.Euler(rotationSpeed);
        spinnerRigidBody.MoveRotation(deltaRotation * spinnerRigidBody.rotation);

        if (GetComponent<SpinDriver>() != null)
        {
            SpinDriver[] spinDrivers = GetComponents<SpinDriver>();

            foreach (SpinDriver spinDriver in spinDrivers)
            {
                spinDriver.objectToDrive.transform.rotation = spinDriver.objectToDrive.transform.rotation 
                    * Quaternion.Euler(Vector3.Scale(spinDriver.driveCoefficient, rotationSpeed));
            }
        }
    }

    private void OnValidate()
    {
        if (spinnerRigidBody == null)
            return;
        if (spinnerRigidBody.isKinematic == false)
        {
            EditorUtility.DisplayDialog("Must be Kinematic", "The Rigidbody on this object must be kinematic for the spinner to work properly.", "Gotcha.");
            spinnerRigidBody.isKinematic = true;
        }
    }

    public void CreateSpinDriver()
    {
        gameObject.AddComponent<SpinDriver>();
    }
}
