using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Index/Material Index")]
public class MaterialIndex : ScriptableObject
{
    public Material blueGoalMaterial;
    public Material redGoalMaterial;
    public Material eitherGoalMaterial;

    public Material blueTapeMaterial;
    public Material redTapeMaterial;

    // The material for tracker objects that show where objects will spawn
    public Material trackerObjectMaterial;
}
