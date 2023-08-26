using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PixelDropper : MonoBehaviour
{
    public int maxWidth = 7;

    public GameObject defaultDropObj;
    public GameObject purplepixel;
    private float offset;
    // Start is called before the first frame update
    void Start()
    {
        offset = defaultDropObj.GetComponentInChildren<MeshCollider>().bounds.max.x * 0.10f;

    }

    IEnumerator DropRowTriple()
    {
        DropTripletRowOdd();
        yield return new WaitForSeconds(1);
        DropTripletRowEven();
        yield return null;
    }

    public void StartDropFill()
    {
        StartCoroutine("DropRow");
    }

    public void StartDropTriplet()
    {
        StartCoroutine("DropRowTriple");
    }

    IEnumerator DropRow()
    {
        int x = 0;
        for(int i =0;i < 75;i++)
        {
            if (x >= maxWidth) { x = 0; }
            else { Drop(x); }
            x++;
            
            yield return new WaitForSeconds(1);
        }
        yield return null;
    }

    public void DropTripletRowOdd()
    {
        Drop(0, purplepixel);
        Drop(1, purplepixel);
        Drop(2);
        Drop(3);
        Drop(4, purplepixel);
        Drop(5, purplepixel);
    }
    public void DropTripletRowEven()
    {
        Drop(0);
        Drop(1, purplepixel);
        Drop(2);
        Drop(3);
        Drop(4, purplepixel);
        Drop(5);
    }

    public void DropRow(int num, bool oddRow = false)
    {
        int start = 0;
        if (oddRow) { start = 1; }
        for (int i = start; i < num+start; i++)
        {
            Drop(i);
        }
    }

    public void Drop(int pos, GameObject prefab = null)
    {
        if(!prefab)
        {
            prefab = defaultDropObj;
        }
        if(pos > maxWidth - 1) { pos = maxWidth - 1; }
        GameObject newPix = GameObject.Instantiate(prefab,transform.parent);
        Vector3 position = transform.localPosition;
        position.x += pos * offset;
        newPix.transform.localPosition = position;
        newPix.transform.localRotation = transform.localRotation;
        newPix.transform.Rotate(0, 32, 0);
        newPix.transform.parent = null;
    }

}
