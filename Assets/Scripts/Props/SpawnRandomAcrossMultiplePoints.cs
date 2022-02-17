using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SpawnRandomAcrossMultiplePoints : ObjectSpawner
{
    Transform[] potentialPositions;
    public int NumberToSpawn;
    // Start is called before the first frame update
    void Start()
    {
        potentialPositions = transform.GetComponentsInChildren<Transform>();
        int numberOfPositionsLeft = potentialPositions.Length;

        List<Transform> positionList = potentialPositions.ToList();

        // Spawn objects at random positions until enough have been spawned;
        while (positionList.Count > potentialPositions.Length - NumberToSpawn)
        {
            int randomLocationIndex = Random.Range(0, positionList.Count);
            GameObject newObject = Instantiate(objectPrefab, transform.parent);
            newObject.transform.position = positionList[randomLocationIndex].position;
            positionList.RemoveAt(randomLocationIndex);
        }

        foreach(Transform thisTransform in potentialPositions)
        {
            thisTransform.gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
