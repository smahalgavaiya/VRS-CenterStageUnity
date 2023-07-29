using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldData : MonoBehaviour
{
    public BuiltInBots botsList;
    public GameObject loadedPropPrefab;
    public static FieldData ins;
    public static BuiltInBots bots {get {return ins.botsList;}}
    // Start is called before the first frame update
    void Awake()
    {
        ins = this;
    }

}
