using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;
using static UnityEditor.PlayerSettings;

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
        int score = 0;
        List<GameObject> scoredObjects = new List<GameObject>();
        for (int i = 0; i < numLines; i++)
        {
            bool threshold = false;
            if (i == 2 || i == 5 || i == 8)
            {
                threshold = true;
            }
            //move to raycast non alloc later.
            RaycastHit[] hits;
            Vector3 rayPos = lineOrigin.position + (lineOrigin.up * (i * vspace));
            Color c = Color.red;
            if(threshold)
            {
                c = Color.blue;
            }
            Debug.DrawRay(rayPos, lineOrigin.right * -0.52f, c);
            hits = Physics.RaycastAll(rayPos, -lineOrigin.right , 0.52f);
            foreach(RaycastHit hit in hits)
            {
                GameObject g = hit.collider.transform.root.gameObject;
                ScoreObjectTypeLink sc = g.GetComponent<ScoreObjectTypeLink>();
                if(sc != null)
                {
                    if (threshold) { score += 10; threshold = false; }//only one pixel may score threshold bonus
                    if(!scoredObjects.Contains(g))
                    {
                        score += 3;//diff points for autonomous
                        scoredObjects.Add(g);
                    }
                    /*int number_of_rays = 6;
                    float totalAngle = 360;

                    float delta = totalAngle / number_of_rays;
                    const float magnitude = 0.1f;
                    for (int h = 0; h< number_of_rays; h++)
                    {
                        var dir = Quaternion.Euler(0, h*delta,0 ) * g.transform.up;
                        Debug.DrawRay(g.transform.position, dir * magnitude, Color.green);
                    }*/
                }
                //prevent scoring same obj twice.
            }
            
        }
        Debug.Log(score);
    }

    private void OnDrawGizmos()
    {
        CastRays();
        
    }
}
