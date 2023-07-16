using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceRandomizationObject : MonoBehaviour
{
    public Team owningTeam;
    public GameObject objectPrefab;
    public Dictionary<string, Transform> randomizationLocations = new Dictionary<string, Transform>();
    // Start is called before the first frame update

    //get all instances of placeR_obj, have a master class that will set randomization and wait on an event from field manager.
    //

    private void Awake()
    {
        randomizationLocations = new Dictionary<string, Transform>();
        foreach (Transform t in transform)
        {
            randomizationLocations.Add(t.name, t);
        }
    }

    public void Place(string loc)
    {
        Awake();
        if(randomizationLocations.ContainsKey(loc))
        {
            GameObject obj = GameObject.Instantiate(objectPrefab);
            obj.transform.position = randomizationLocations[loc].position;
            obj.transform.parent = transform;
        }
    }
}
