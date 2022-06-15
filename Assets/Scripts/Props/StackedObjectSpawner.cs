using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StackedObjectSpawner : ObjectSpawner
{
    public int objectCount;
    public List<GameObject> ghostObjects;
    // Start is called before the first frame update
    void Start()
    {
        ClearGhostObjects();
        SpawnNextObject();
    }

    void ClearGhostObjects()
    {
        foreach(GameObject ghostObject in ghostObjects)
        {
            ghostObject.SetActive(false);
        }
    }

    public void SpawnNextObject()
    {
        if (objectCount < 1)
            return;

        GameObject newObject = Instantiate(objectPrefab, transform.parent);
        newObject.transform.position = transform.position;

        objectCount--;
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
