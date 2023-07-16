using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackdropScorer : MonoBehaviour
{
    public Team team;
    public Transform lineOrigin;
    public float verticalSpacing;
    public int numLines = 10;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Debug.DrawRay(lineOrigin.position, lineOrigin.up * 100, Color.red);
    }

    public void CastRays()
    {
        float vspace = verticalSpacing * 0.01f;
        Vector3 curRayStart = lineOrigin.localPosition;
        for (int i = 0; i < numLines; i++)
        {
            if (i % 2 == 0) { /*is threshold*/}
            //move to raycast non alloc later.
            RaycastHit[] hits;
            //curRayStart.y = lineOrigin.position.y + (lineOrigin.position.y * (i * verticalSpacing));
            //curRayStart.z = lineOrigin.position.z + (lineOrigin.position.z * (i * verticalSpacing));
            Vector3 rayPos = lineOrigin.position + (lineOrigin.up * (i * vspace));
            Debug.DrawRay(rayPos, lineOrigin.right * -0.52f, Color.red);
            hits = Physics.RaycastAll(rayPos, -lineOrigin.right , 0.52f);
            foreach(RaycastHit hit in hits)
            {
                Debug.DrawLine(hit.point, hit.normal, Color.green);
            }
        }
    }

    private void OnDrawGizmos()
    {
        CastRays();
        
    }
}
