using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class testScoring : MonoBehaviour
{
    Dictionary<string,ScoreTracker> scores = new Dictionary<string,ScoreTracker>();
    SelectBotOptions botOptions;

    [SetUp]
    public void SetUp()
    {
        ScoreTracker[] trackers =  Resources.LoadAll<ScoreTracker>("");
        scores.Clear();
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
        yield return TestConeScoring(TeamColor.Blue,80);

    }

    [UnityTest]
    public IEnumerator TestBlueScoringGround()
    {
        yield return TestConeScoring(TeamColor.Blue,18,true,false);

    }
    [UnityTest]
    public IEnumerator TestBlueScoringAll()
    {
        yield return TestConeScoring(TeamColor.Blue,98, true,true);

    }

    [UnityTest]
    public IEnumerator TestRedScoringPoles()
    {
        yield return TestConeScoring(TeamColor.Red,80);

    }

    [UnityTest]
    public IEnumerator TestRedScoringGround()
    {
        yield return TestConeScoring(TeamColor.Red,18,true, false);

    }
    [UnityTest]
    public IEnumerator TestRedScoringAll()
    {
        yield return TestConeScoring(TeamColor.Red, 98,true, true);

    }


    public IEnumerator TestBotSpawn()
    {
        botOptions = GameObject.FindObjectOfType<SelectBotOptions>();
        botOptions.StartGame();
        DriveReceiverSpinningWheels wheels = botOptions.spawnedBot.GetComponent<DriveReceiverSpinningWheels>();
        yield return new WaitForSeconds(0.5f);
        botOptions.gameObject.SetActive(false);
        for (int i = 0; i < 1209; i++)
        {
            wheels.frontLeft.driveAmount.x = 1;
            wheels.frontRight.driveAmount.x = 1;
            yield return new WaitForEndOfFrame();
        }
        

    }

    public IEnumerator TestConeScoring(TeamColor color, int correctScore, bool testGround=false,bool testPoles=true)
    {

        SelectBotOptions botgui = GameObject.FindObjectOfType<SelectBotOptions>();
        botgui.gameObject.SetActive(false);
        yield return new WaitForSeconds(0.5f);
        DropCone cone = GameObject.FindObjectOfType<DropCone>();
        cone.HeightOffset = 1f;
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
        ScoreTracker otherScore = scores["Blue"];
        if(color == TeamColor.Blue) { score = scores["Blue"]; otherScore = scores["Red"]; }

        Assert.AreEqual(correctScore, score.Score,"Correct Score for Team?");
        Assert.AreEqual(0, otherScore.Score,"Other team shouldnt have any points");
    }
}
