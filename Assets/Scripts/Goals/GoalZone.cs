using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(GoalZoneColorSwitcher))]
public class GoalZone : MonoBehaviour
{
    public Material redZoneMaterial, blueZoneMaterial;

    public bool hideOnPlay;

    public ScoreZone scoreZone;
    // Start is called before the first frame update
    void Start()
    {
        if (hideOnPlay)
        {
            GetComponent<MeshRenderer>().enabled = false;
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnValidate()
    {
        Material material;
        if (scoreZone == ScoreZone.blue)
            material = blueZoneMaterial;
        else
            material = redZoneMaterial;

        GetComponent<GoalZoneColorSwitcher>().SetColor(material);
    }
}

public enum ScoreZone
{
    blue,
    red
}
