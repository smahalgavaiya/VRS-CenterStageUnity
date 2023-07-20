using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
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
        int tripletsFound = 0;
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
                    if(sc.ScoreObjectType_.name == "WhitePixel") { continue; }
                    bool found = DetectTriplet(g);
                    if (found) { tripletsFound++; }
                }
                //prevent scoring same obj twice.
            }
            //if(tripletUnits == 3) { tripletsFound++; }
           //run through triplets after initial score?
        }

        Debug.Log("Triplets:" + tripletsFound /3);
        Debug.Log(score);
    }

    public Dictionary<int,ScoreObjectTypeLink> GetNeighbors(GameObject obj)
    {
        RaycastHit[] hits;
        int number_of_rays = 6;
        float totalAngle = 360;

        float delta = totalAngle / number_of_rays;
        const float magnitude = 0.05f;
        Dictionary<int,ScoreObjectTypeLink> neighbors = new Dictionary<int,ScoreObjectTypeLink>();
        for (int h = 0; h < number_of_rays; h++)
        {

            Quaternion q = Quaternion.AngleAxis(h * delta + 31, obj.transform.up);
            var dir = q * obj.transform.right;

            //Debug.DrawRay(obj.transform.position, dir * magnitude, Color.red);
            hits = Physics.RaycastAll(obj.transform.position, dir, magnitude);
            foreach (RaycastHit hit in hits)
            {
                GameObject g = hit.collider.transform.root.gameObject;
                ScoreObjectTypeLink sc = g.GetComponent<ScoreObjectTypeLink>();
                if (sc != null && sc.ScoreObjectType_.name != "WhitePixel")
                {
                    Debug.DrawRay(obj.transform.position, dir * magnitude, Color.green);
                    neighbors.Add(h, sc);
                }
            }
        }
        return neighbors;
    }

    public bool DetectTriplet(GameObject obj)
    {
        Dictionary<int, ScoreObjectTypeLink> neighbors = GetNeighbors(obj);
        List<int> potentialTriplets = new List<int>();
        foreach(KeyValuePair<int,ScoreObjectTypeLink> sc in neighbors)
        {
            //Debug.DrawRay(obj.transform.position, dir * magnitude, Color.green);
            potentialTriplets.Add(sc.Key);
            if (potentialTriplets.Count > 2) { return false; }
        }
        if (potentialTriplets.Count == 2)
        {
            int diff = Mathf.Abs(potentialTriplets[0] - potentialTriplets[1]);
            if (diff == 1 || diff == 5)
            {
                int count_n1 = this.GetNeighbors(neighbors[potentialTriplets[0]].gameObject).Count;
                int count_n2 = this.GetNeighbors(neighbors[potentialTriplets[1]].gameObject).Count;
                if(count_n1 > 2 || count_n2 > 2) { return false; }
                //check same color or all diff colors!!
                Debug.DrawRay(obj.transform.position, obj.transform.up * 0.2f, Color.yellow);
                return true;
            }
            else
            {
                return false;
            }
        }

        return false;
    }

    private void OnDrawGizmos()
    {
        CastRays();
        
    }
}
