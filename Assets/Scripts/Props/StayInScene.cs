using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This moves an object to the root transform, keeping it enabled in the scene
public class StayInScene : MonoBehaviour
{
    Vector3 initialScaleParent, initialScaleObject;
    bool scaleSet = false;

    private void Awake()
    {
        initialScaleParent = transform.parent.localScale;
        initialScaleObject = transform.localScale;
    }
    private void Start()
    {
        gameObject.transform.SetParent(transform.root);
        gameObject.SetActive(true);
    }
    private void Update()
    {
        if (!scaleSet)
        {
            transform.localScale = Vector3.Scale(initialScaleObject, initialScaleParent);
            scaleSet = true;
        }
    }
}
