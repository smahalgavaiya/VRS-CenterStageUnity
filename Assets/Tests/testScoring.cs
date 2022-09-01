using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class testScoring : MonoBehaviour
{
    Dictionary<string,ScoreTracker> scores = new Dictionary<string,ScoreTracker>();

    [SetUp]
    public void SetUp()
    {
        ScoreTracker[] trackers =  Resources.LoadAll<ScoreTracker>("");
        foreach (ScoreTracker tracker in trackers)
        {
            if (tracker.name.Contains("Red"))
            {
                scores.Add("Red", tracker);
            }
            else
            {
                scores.Add("Blue", tracker);
            }
        }
        SceneManager.LoadScene("PowerPlayNewBots",LoadSceneMode.Single);
        
    }

    [TearDown]
    public void Teardown()
    {
        //SceneManager.
        //Object.Destroy(game.gameObject);
    }

    [UnityTest]
    public IEnumerator TestBlueScoringPoles()
    {
        yield return TestConeScoring(TeamColor.Blue,16);

    }

    [UnityTest]
    public IEnumerator TestBlueScoringGround()
    {
        yield return TestConeScoring(TeamColor.Blue,4,true,false);

    }
    [UnityTest]
    public IEnumerator TestBlueScoringAll()
    {
        yield return TestConeScoring(TeamColor.Blue,14,true,true);

    }

    [UnityTest]
    public IEnumerator TestRedScoringPoles()
    {
        yield return TestConeScoring(TeamColor.Red,16);

    }

    [UnityTest]
    public IEnumerator TestRedScoringGround()
    {
        yield return TestConeScoring(TeamColor.Red,4,true, false);

    }
    [UnityTest]
    public IEnumerator TestRedScoringAll()
    {
        yield return TestConeScoring(TeamColor.Red, 14,true, true);

    }

    public IEnumerator TestConeScoring(TeamColor color, int correctScore, bool testGround=false,bool testPoles=true)
    {
        SelectBotOptions botgui = GameObject.FindObjectOfType<SelectBotOptions>();
        botgui.gameObject.SetActive(false);
        yield return new WaitForSeconds(0.5f);
        DropCone cone = GameObject.FindObjectOfType<DropCone>();
        cone.color = color; 
        JunctionCapper[] scoringLocs = GameObject.FindObjectsOfType<JunctionCapper>();
        foreach(JunctionCapper loc in scoringLocs)
        {
            string scoreObjName = loc.transform.parent.parent.parent.name;
            if (scoreObjName.Contains("Ground") && !testGround) { continue; }
            if (!scoreObjName.Contains("Ground") && !testPoles) { continue; }
            cone.Drop(loc.gameObject);
            yield return new WaitForSeconds(.1f);
        }
        yield return new WaitForSeconds(2);
        ScoreTracker score = scores["Red"];
        if(color == TeamColor.Blue) { score = scores["Blue"]; }

        Assert.AreEqual(correctScore, score.Score);
    }
}
