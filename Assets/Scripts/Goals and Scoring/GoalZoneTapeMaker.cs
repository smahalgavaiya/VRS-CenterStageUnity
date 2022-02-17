using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This class draws tape on the floor automatically around Goal Zones
[ExecuteInEditMode]
public class GoalZoneTapeMaker : MonoBehaviour
{
    [Range(0,4)]
    public int numberOfSides;

    private GameObject[] tapeSides = new GameObject[4];
    public float tapeWidth;
    public float tapeHeight;
    public MaterialIndex materialIndex;

    int previousNumberOfSides;
    private void OnEnable()
    {
        GetTapeSides();
    }
    // Start is called before the first frame update
    void Start()
    {
    }

    void GetTapeSides()
    {
        for (int i = 0; i < 4; i++)
        {
            tapeSides[i] = transform.GetChild(i).gameObject;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // If these are not defined
        if (tapeSides[0] == null)
            GetTapeSides();

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
                                    new Vector3(tapeWidth / gameObject.transform.lossyScale.x , tapeHeight, 
                                        1 + (2 * tapeWidth / gameObject.transform.localScale.z));
                                break;
                            case 1:
                                tapeSides[i].transform.position += 
                                    new Vector3(0, 0, gameObject.transform.lossyScale.z / 2 + tapeWidth / 2);
                                tapeSides[i].transform.localScale =
                                    new Vector3(1 + (2 * tapeWidth / gameObject.transform.localScale.x), tapeHeight, 
                                        tapeWidth / gameObject.transform.lossyScale.z);
                                break;
                            case 2:
                                tapeSides[i].transform.position -= 
                                    new Vector3(gameObject.transform.lossyScale.x / 2 + tapeWidth / 2, 0, 0);
                                tapeSides[i].transform.localScale =
                                    new Vector3(tapeWidth / gameObject.transform.lossyScale.x, tapeHeight, 
                                        1 + (2 * tapeWidth / gameObject.transform.localScale.z));
                                break;
                            case 3:
                                tapeSides[i].transform.position -= 
                                    new Vector3(0, 0, gameObject.transform.lossyScale.z / 2 + tapeWidth / 2);
                                tapeSides[i].transform.localScale =
                                    new Vector3(1 + (2 * tapeWidth / gameObject.transform.localScale.x), tapeHeight, 
                                        tapeWidth / gameObject.transform.lossyScale.z);
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

    // Set tape color depending on team
    public void SetTapeColor(ScoreZoneColor scoreZone)
    {
        if (tapeSides[0] == null)
            GetTapeSides();

        foreach (GameObject tapeSide in tapeSides)
        {
            switch (scoreZone)
            {
                case ScoreZoneColor.Blue:
                    tapeSide.GetComponent<Renderer>().material = materialIndex.blueTapeMaterial;
                    break;
                case ScoreZoneColor.Red:
                    tapeSide.GetComponent<Renderer>().material = materialIndex.redTapeMaterial;
                    break;
            }
        }
    }
}
