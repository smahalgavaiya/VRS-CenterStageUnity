using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class SingleTapeMaker : MonoBehaviour
{
    RaycastHit[] allHits;
    [SerializeField]
    GameObject tapePiece;
    bool floorFound = false;
    public Vector3 tapeScale;

    // Start is called before the first frame update
    void Start()
    {
        DetectFloorDrawTape();
    }

    // Update is called once per frame
    void Update()
    {
        DetectFloorDrawTape();
    }

    public void DetectFloorDrawTape()
    {
        floorFound = false;
        allHits = Physics.RaycastAll(transform.position, Vector3.down);
        for (int i = 0; i < allHits.Length; i++)
        {
            if (allHits[i].collider.tag == "Floor")
            {
                if (tapePiece == null)
                {
                    tapePiece = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    tapePiece.name = "tape";
                    tapePiece.transform.SetParent(transform);
                    tapePiece.transform.position = Vector3.zero;
                    tapePiece.GetComponent<BoxCollider>().enabled = false;
                    tapePiece.AddComponent<StayInScene>();
                }

                tapePiece.transform.localScale = tapeScale;
                tapePiece.transform.position = allHits[i].point;
                floorFound = true;
            }
        }

        if (!floorFound)
        {
            if (tapePiece != null)
            {
                DestroyImmediate(tapePiece);
            }
        }
    }

    private void OnDestroy()
    {
        DestroyImmediate(tapePiece);
    }
}
