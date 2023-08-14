using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System.Linq;

public class BackdropScorer : MonoBehaviour
{
    public TeamColor team;
    public Transform lineOrigin;
    public float verticalSpacing;
    public int numLines = 10;
    private int lastScore = 0;
    public ScoringGuide backdropPixels;

    private int[] scoreIndexPixels = { -1,-1,-1,-1};
    private int scoreIndexThreshold = -1;
    private int scoreIndexTriplet = -1;
    // Start is called before the first frame update
    private GameTimeManager gametime;

    //Should have lists for scored triplets, pixels, etc.
    void Start()
    {
        gametime = FindFirstObjectByType<GameTimeManager>();
        InvokeRepeating("CastRaysScore",1,5);
    }

    // Update is called once per frame
    void Update()
    {
        //CastRays();
    }

    private void OnCollisionEnter(Collision collision)
    {
        //CastRays();
    }

    public void CastRaysScore()
    {
        CastRays();
    }

    public void CastRays(bool noScore = false)
    {
        if (!noScore && !gametime.IsRunning) { return; }
        //CancelInvoke();
        float vspace = verticalSpacing * 0.01f;
        Vector3 curRayStart = lineOrigin.localPosition;
        int score = 0;
        List<GameObject> scoredObjects = new List<GameObject>();
        int tripletsFound = 0;
        //if (!noScore) { ScoringManager.ScoreEvent(team, -lastScore, "Resetting Board", gameObject); }
        int pixelsScore = 0;

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
                    Debug.DrawRay(sc.gameObject.transform.position, hit.collider.transform.root.up * -0.02f, Color.white);
                    if (threshold) 
                    { 
                        score += 10;
                        if (!noScore) 
                        { 
                            if (scoreIndexThreshold > -1)
                            {
                                ScoringManager.OverwriteScore(scoreIndexThreshold,team, 10, "Threshold Reached", gameObject);
                            }
                            else
                            {
                                scoreIndexThreshold = ScoringManager.AddScore(team,10, "Threshold Reached", gameObject);
                            }
                        }
                        threshold = false; 
                    }//only one pixel may score threshold bonus
                    if(!scoredObjects.Contains(g))
                    {
                        
                        pixelsScore += 1;//diff points for autonomous
                        
                        scoredObjects.Add(g);
                    }
                    if(sc.ScoreObjectType_.name == "WhitePixel") { continue; }
                    bool found = DetectTriplet(sc);
                    if (found) { tripletsFound++; }
                }
                //prevent scoring same obj twice.
            }
            //if(tripletUnits == 3) { tripletsFound++; }
           //run through triplets after initial score?
        }
        if (!noScore)
        {
            
            int pixelsIdx = scoreIndexPixels[gametime.currentSession.globalInt];
            if(pixelsIdx > -1)
            {
                ScoringManager.OverwriteScore(pixelsIdx,team, backdropPixels, 0, "Pixels On Board", gameObject, pixelsScore);
            }
            else
            {
                scoreIndexPixels[gametime.currentSession.globalInt] = ScoringManager.AddScore(team, backdropPixels, 0, "Pixels On Board", gameObject, pixelsScore);
            }
            
            if (scoreIndexTriplet > -1)
            {
                ScoringManager.OverwriteScore(scoreIndexThreshold, team, (tripletsFound/3)*10, "Triplets x"+tripletsFound/3, gameObject);
            }
            else
            {
                scoreIndexTriplet = ScoringManager.AddScore(team, (tripletsFound / 3) * 10, "Triplets x" + tripletsFound / 3, gameObject);
            }
            lastScore = score + pixelsScore;
        }
        
        if (noScore) { Debug.Log("Triplets:" + tripletsFound / 3); Debug.Log(score); }
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

    public bool DetectTriplet(ScoreObjectTypeLink scoringObj)
    {
        GameObject obj = scoringObj.gameObject;


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
                //Must be Touching Check
                int count_n1 = this.GetNeighbors(neighbors[potentialTriplets[0]].gameObject).Count;
                int count_n2 = this.GetNeighbors(neighbors[potentialTriplets[1]].gameObject).Count;

                if (count_n1 > 2 || count_n2 > 2) { return false; }

                //Color Checks
                ScoreObjectTypeLink[] triplet = { scoringObj, neighbors[potentialTriplets[0]], neighbors[potentialTriplets[1]] };

                bool allSame = true;
                bool allDifferent = false;
                string firstName = "";
                string lastName = "";
                foreach (ScoreObjectTypeLink s in triplet)
                {

                    if (firstName == "") { firstName = s.ScoreObjectType_.name; }
                    if (firstName != s.ScoreObjectType_.name && !allSame && lastName != s.ScoreObjectType_.name) { allDifferent = true; }
                    if (firstName != s.ScoreObjectType_.name) { allSame = false; }
                    lastName = s.ScoreObjectType_.name;
                }

                if (!allSame && !allDifferent) { return false; }
                //Color Check

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
        CastRays(true);
        
    }
}
