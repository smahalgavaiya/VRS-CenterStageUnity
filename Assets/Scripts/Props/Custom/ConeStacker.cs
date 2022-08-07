using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class ConeStacker : MonoBehaviour
{
    GameObject topCone, secondCone;
    public GameObject TopCone { get => topCone; set => topCone = value; }
    public GameObject SecondCone { get => secondCone; set => secondCone = value; }

    [Range(1,10)]
    [SerializeField] int numberOfConesInStack;
    int numberOfEnabledCones;

    List<GameObject> cones;

    // Start is called before the first frame update
    void Awake()
    {
        GetCones();
    }

    void GetCones()
    {
        if (cones == null)
            cones = new List<GameObject>();
        cones.Clear();

        for(int i = 0; i < transform.childCount; i++)
        {
            cones.Add(transform.GetChild(i).gameObject);
        }

        TopCone = cones[0];
        secondCone = cones[1];
    }
    // Update is called once per frame
    void Update()
    {
        if (cones == null)
            GetCones();
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
            cones[i].GetComponent<Cone>().ConeMeshObject.GetComponent<MeshCollider>().enabled = false;
            cones[i].GetComponent<Cone>().JunctionSliderObject.SetActive(false);
            cones[i].GetComponent<Cone>().ConeBaseForStacking.SetActive(true);
        }
        for (int i = numberOfConesInStack; i < cones.Count; i++)
        {
            cones[i].SetActive(false);
        }

        TopCone.GetComponent<Cone>().ConeMeshObject.GetComponent<MeshCollider>().enabled = true;
        TopCone.GetComponent<Cone>().JunctionSliderObject.SetActive(true);

        ConeStackColorSwitcher coneStackColorSwitcher = GetComponent<ConeStackColorSwitcher>();
        coneStackColorSwitcher.GetChildObjects();

        numberOfEnabledCones = numberOfConesInStack;
    }

    public void ReleaseTopCone()
    {
        topCone.GetComponent<Cone>().ConeBaseForStacking.SetActive(false);

        cones.Remove(topCone);
        topCone = cones[0];
        secondCone = cones[1];
    }
}
