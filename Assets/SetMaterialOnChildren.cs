using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetMaterialOnChildren : MonoBehaviour
{
    [SerializeField] Material material;

    public void SetColor()
    {
        foreach (Transform t in gameObject.transform)
        {
            Renderer r = t.gameObject.GetComponent<Renderer>();
            if (r) { r.material = material; }
        }
    }

    public void SetColor(GameObject obj)
    {
        if(obj == gameObject)
        {
            SetColor();
        }
    }

    public void SetColorOnNew(GameObject obj)
    {
        foreach (Transform t in obj.transform)
        {
            Renderer[] renderers = t.gameObject.GetComponentsInChildren<Renderer>();
            foreach(Renderer r in renderers)
            {
                r.material = material;
            }
        }
    }
}

