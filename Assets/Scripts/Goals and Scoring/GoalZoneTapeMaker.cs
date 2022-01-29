using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This class draws tape on the floor automatically around Goal Zones
[ExecuteInEditMode]
public class GoalZoneTapeMaker : MonoBehaviour
{
    public GameObject[] tapeSides;
    public float tapeWidth;
    public float tapeHeight;
    public MaterialIndex materialIndex;

    [Range(0,4)]
    public int numberOfSides;
    int previousNumberOfSides;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

        CheckCollisionWithFloorMaybeActivateTape();
        
        // Check if the number of sides has changed
        if (previousNumberOfSides != numberOfSides)
        {
            for (int i = 0; i < tapeSides.Length; i++)
            {
                tapeSides[i].SetActive(false);
            }
            CheckCollisionWithFloorMaybeActivateTape();
        }

        previousNumberOfSides = numberOfSides;
        
    }
    public void CheckCollisionWithFloorMaybeActivateTape()
    {
        Collider[] overlappingColliders;
        overlappingColliders = Physics.OverlapBox(gameObject.transform.position, gameObject.transform.lossyScale / 2);

        bool foundFloorCollider = false;

        foreach (Collider collider in overlappingColliders)
        {
            if (collider.gameObject.tag == "Floor")
            {
                foundFloorCollider = true;
                if (!tapeSides[0].activeSelf) // If the tape hasn't been activated
                {
                    for (int i = 0; i < numberOfSides; i++)
                    {
                        tapeSides[i].SetActive(true);
                    }
                }

                else if (tapeSides[0] != null)
                {
                    for (int i = 0; i < numberOfSides; i++)
                    {
                        tapeSides[i].transform.position = new Vector3(gameObject.transform.position.x,
                            collider.transform.position.y, gameObject.transform.position.z);

                        switch(i) {
                            case 0:
                                tapeSides[i].transform.position += 
                                    new Vector3(gameObject.transform.lossyScale.x / 2 + tapeWidth / 2, 0, 0);
                                tapeSides[i].transform.localScale =
                                    new Vector3(tapeWidth, tapeHeight, gameObject.transform.localScale.z + tapeWidth * 2);
                                break;
                            case 1:
                                tapeSides[i].transform.position += 
                                    new Vector3(0, 0, gameObject.transform.lossyScale.z / 2 + tapeWidth / 2);
                                tapeSides[i].transform.localScale =
                                    new Vector3(gameObject.transform.localScale.x + tapeWidth * 2, tapeHeight, tapeWidth);
                                break;
                            case 2:
                                tapeSides[i].transform.position -= 
                                    new Vector3(gameObject.transform.lossyScale.x / 2 + tapeWidth / 2, 0, 0);
                                tapeSides[i].transform.localScale =
                                    new Vector3(tapeWidth, tapeHeight, gameObject.transform.localScale.z + tapeWidth * 2);
                                break;
                            case 3:
                                tapeSides[i].transform.position -= 
                                    new Vector3(0, 0, gameObject.transform.lossyScale.z / 2 + tapeWidth / 2);
                                tapeSides[i].transform.localScale =
                                    new Vector3(gameObject.transform.localScale.x + tapeWidth * 2, tapeHeight, tapeWidth);
                                break;
                        }
                    }
                }
            }
        }
        
        if (foundFloorCollider == false && tapeSides[0] != null) // If it was on the floor, but isn't any longer.
        {
            for (int i = 0; i < tapeSides.Length; i++)
            {
                tapeSides[i].SetActive(false);
            }
        }
    }

    public void SetTapeColor(ScoreZone scoreZone)
    {
        foreach (GameObject tapeSide in tapeSides)
        {
            switch (scoreZone)
            {
                case ScoreZone.Blue:
                    tapeSide.GetComponent<Renderer>().material = materialIndex.blueTapeMaterial;
                    break;
                case ScoreZone.Red:
                    tapeSide.GetComponent<Renderer>().material = materialIndex.redTapeMaterial;
                    break;
            }
        }
    }
}
