using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class ConeStacker : MonoBehaviour
{
    [SerializeField] GameObject topCone;

    [Range(1,10)]
    [SerializeField] int numberOfConesInStack;
    int numberOfEnabledCones;

    [SerializeField] List<GameObject> cones;


    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (numberOfEnabledCones != numberOfConesInStack)
        {
            ResetBase();
        }
    }

    // Resets the base upon which the top cone rests
    void ResetBase()
    {
        for (int i = 0; i < numberOfConesInStack; i++)
        {
            cones[i].SetActive(true);
        }
        for (int i = numberOfConesInStack; i < cones.Count; i++)
        {
            cones[i].SetActive(false);
        }

        numberOfEnabledCones = numberOfConesInStack;
    }

    void TurnCollidersOnOrOff(GameObject cone, bool onOrOff)
    {
        foreach(Collider collider in cone.GetComponentsInChildren<Collider>())
        {
            collider.enabled = onOrOff;
        }
    }

}
