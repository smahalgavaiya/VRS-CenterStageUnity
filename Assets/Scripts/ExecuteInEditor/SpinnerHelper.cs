using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[ExecuteInEditMode]
public class SpinnerHelper : MonoBehaviour
{
    Rigidbody spinnerRigidbody;
    // Start is called before the first frame update
    void Start()
    {
        spinnerRigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (spinnerRigidbody.isKinematic == false)
        {
            EditorUtility.DisplayDialog("Must be Kinematic", "The Rigidbody on this object must be kinematic for the spinner to work properly.", "Gotcha.");
            spinnerRigidbody.isKinematic = true;
        }
    }
}
