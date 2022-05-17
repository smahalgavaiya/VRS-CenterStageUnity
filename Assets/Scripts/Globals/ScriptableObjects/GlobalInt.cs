using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Globals/Global Int")]
public class GlobalInt : ScriptableObject
{
    public int globalInt;

    public void ResetIntToZero()
    {
        globalInt = 0;
    }
}
