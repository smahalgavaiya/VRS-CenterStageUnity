using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomizationController : MonoBehaviour
{
    public List<string> randomLocs = new List<string>();
    private int chosenLoc;

    private void Awake()
    {
        FieldManager.OnFieldSetup += SetRandomization;
        SetRandomization();
    }
    /*static RandomizationController()
    {
        FieldManager.OnFieldSetup += SetRandomization;
    }*/

    public void SetRandomization()
    {
        chosenLoc = Random.Range(0, randomLocs.Count);
        PlaceRandomizationObject[] objs = FindObjectsByType<PlaceRandomizationObject>(FindObjectsInactive.Include,FindObjectsSortMode.None);
        foreach(PlaceRandomizationObject obj in objs)
        {
            obj.Place(randomLocs[chosenLoc]);
        }
    }
}
