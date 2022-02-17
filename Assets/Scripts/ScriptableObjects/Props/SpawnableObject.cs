using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Props/SpawnableObject")]
public class SpawnableObject : ScriptableObject
{
    public GameObject spawnableGameObject;
    public int quantityToSpawn;
}
