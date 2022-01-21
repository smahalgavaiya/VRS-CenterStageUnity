using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[ExecuteInEditMode]
public class SpinnerHelper : MonoBehaviour
{
    Rigidbody rigidbody;
    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (rigidbody.isKinematic == false)
        {
            EditorUtility.DisplayDialog("Must be Kinematic", "The Rigidbody on this object must be kinematic for the spinner to work properly.", "Gotcha.");
            rigidbody.isKinematic = true;
        }
    }
}
