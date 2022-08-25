using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class DropCone : MonoBehaviour
{

    public GameObject BlueConesParent;
    public GameObject RedConesParent;

    List<GameObject> redCones;
    List<GameObject> blueCones;

    public TeamColor color = TeamColor.Blue;
    public float HeightOffset;
    public float XOffset;
    public float ZOffset;

    // Start is called before the first frame update
    void Start()
    {
        blueCones = new List<GameObject>();
        redCones = new List<GameObject>();
        foreach (Cone cone in BlueConesParent.GetComponentsInChildren<Cone>())
        {
            blueCones.Add(cone.gameObject);
        }
        foreach (Cone cone in RedConesParent.GetComponentsInChildren<Cone>())
        {
            redCones.Add(cone.gameObject);
        }


    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("0"))
        {

            GameObject newCone;

            if (color == TeamColor.Blue)
            {
                newCone = blueCones[0];
                blueCones.RemoveAt(0);
            }
            else if (color == TeamColor.Red)
            {
                newCone = redCones[0];
                redCones.RemoveAt(0);
            }
            else
            {
                newCone = redCones[0];
                redCones.RemoveAt(0);
            }

            newCone.GetComponent<Rigidbody>().isKinematic = false;
            Vector3 newPos = Selection.activeGameObject.transform.position;
            newPos.y += HeightOffset;
            newPos.x += XOffset;
            newPos.z += ZOffset;

            newCone.transform.position = newPos;

            //GameObject obj = Selection.activeGameObject;
            //Vector3 temp = obj.transform.position;
            //temp.y += 0.5f;
            //GameObject newObj = Object.Instantiate(RedConeTemplate, obj.transform.position, obj.transform.rotation );
            //newObj.GetComponent<Rigidbody>().isKinematic = false;
        }

    }
}