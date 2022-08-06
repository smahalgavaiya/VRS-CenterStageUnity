using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircularObjectArray : MonoBehaviour
{
    [SerializeField] GameObject arrayObject;
    [SerializeField] int numberOfSides;

    GameObject[] objectArray;
    // Start is called before the first frame update
    void OnEnable()
    {
        if (objectArray == null)
            GenerateArray();
    }

    // Update is called once per frame
    void Update()
    {
    }

    void GenerateArray()
    {
        objectArray = new GameObject[numberOfSides];

        for (int i = 0; i < objectArray.Length; i++)
        {
            GameObject newArrayObject = Instantiate(arrayObject);
            objectArray[i] = newArrayObject;
            newArrayObject.transform.SetParent(transform);
            newArrayObject.transform.localPosition = Vector3.zero;
            newArrayObject.transform.localScale = Vector3.one;

            float amountToRotate = 360f / numberOfSides * i;

            newArrayObject.transform.localEulerAngles = new Vector3(0, amountToRotate, 0);

        }
    }

}
