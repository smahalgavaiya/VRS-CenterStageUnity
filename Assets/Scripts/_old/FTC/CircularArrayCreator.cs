using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircularArrayCreator : MonoBehaviour
{
    public GameObject pieceToRotate;
    public int numberOfRotations;

    // Start is called before the first frame update
    void Start()
    {
        CreateCircleArray();
    }

    void CreateCircleArray()
    {
        float angle = 360 / numberOfRotations;

        for(int i = 0; i < numberOfRotations; i++)
        {
            GameObject newBottomPiece = Instantiate(pieceToRotate);
            newBottomPiece.transform.parent = pieceToRotate.transform.parent;
            newBottomPiece.transform.localPosition = pieceToRotate.transform.localPosition;
            newBottomPiece.transform.localRotation = pieceToRotate.transform.localRotation;
            newBottomPiece.transform.localScale = pieceToRotate.transform.localScale;
            newBottomPiece.transform.Rotate(0, angle * i, 0, Space.Self);
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
