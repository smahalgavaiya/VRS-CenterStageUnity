using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Spawns a single object at runtime
public class SingleObjectSpawner : ObjectSpawner
{
    // Start is called before the first frame update
    void Start()
    {
        GameObject newObject = Instantiate(objectPrefab, transform.parent);
        newObject.transform.position = transform.position;
        this.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
