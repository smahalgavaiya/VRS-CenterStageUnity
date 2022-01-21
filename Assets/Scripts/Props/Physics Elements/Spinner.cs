using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(SpinnerHelper))]
public class Spinner : MonoBehaviour
{
    Rigidbody rigidbody;
    public Vector3 rotationSpeed;
    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        rigidbody.isKinematic = true;
    }

    // Update is called once per frame
    void Update()
    {
        rigidbody.MoveRotation(Quaternion.Euler(rigidbody.rotation.eulerAngles.x + rotationSpeed.x, 
            rigidbody.rotation.eulerAngles.y + rotationSpeed.y,
            rigidbody.rotation.eulerAngles.z + rotationSpeed.z));
    }

    private void OnValidate()
    {
        if (rigidbody.isKinematic == false)
        {
            EditorUtility.DisplayDialog("Must be Kinematic", "The Rigidbody on this object must be kinematic for the spinner to work properly.", "Gotcha.");
            rigidbody.isKinematic = true;
        }
    }
}
