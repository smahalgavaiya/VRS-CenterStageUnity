using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class GoalZoneColorSwitcher : MonoBehaviour
{
    public void SetColor(Material material)
    {
        GetComponent<Renderer>().material = material;
    }
}
