using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Photon.Pun.UtilityScripts.PunTeams;
using UnityEngine.SocialPlatforms.Impl;

public class DistanceToFloor : MonoBehaviour
{
    public float groundedDist = 0.1f;
    [HideInInspector]
    public bool isGrounded = true;
    private float distance = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit[] hits;
        Vector3 rayPos = transform.position;
        Debug.DrawRay(rayPos, transform.forward * 0.52f,Color.red);
        hits = Physics.RaycastAll(rayPos, transform.forward, 0.52f);
        foreach (RaycastHit hit in hits)
        {

            GameObject g = hit.collider.transform.gameObject;
            if(g.tag.ToLower() == "floor")
            {
                float dist = Vector3.Distance(transform.position, hit.point);
                distance = dist;
                if(dist > groundedDist) { isGrounded = false; return; }

            }
        }
        isGrounded = true;
    }
}
