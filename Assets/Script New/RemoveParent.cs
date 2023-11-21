using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoveParent : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        foreach(Transform child in transform)
        {
            child.parent = null;
        }
        Invoke("Die", 1);
    }

    void Die()
    {
        Destroy(gameObject);
    }
}
