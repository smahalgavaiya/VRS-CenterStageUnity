using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class TestConeScoring : MonoBehaviour
{

    [SetUp]
    public void SetUp()
    {
        SceneManager.sceneLoaded += Loaded;
        SceneManager.LoadScene("PowerPlayNewBots",LoadSceneMode.Single);
    }

    public void Loaded(Scene scene, LoadSceneMode mode)
    {
        TestHelper.ReadyTest();
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
        yield return RunConeScoring(TeamColor.Blue,60);

    }

    [UnityTest]
    public IEnumerator TestBlueScoringGround()
    {
        yield return RunConeScoring(TeamColor.Blue,18,true,false);

    }
    [UnityTest]
    public IEnumerator TestBlueScoringAll()
    {
        yield return RunConeScoring(TeamColor.Blue,78, true,true);

    }

    [UnityTest]
    public IEnumerator TestRedScoringPoles()
    {
        yield return RunConeScoring(TeamColor.Red,60);

    }

    [UnityTest]
    public IEnumerator TestRedScoringGround()
    {
        yield return RunConeScoring(TeamColor.Red,18,true, false);

    }
    [UnityTest]
    public IEnumerator TestRedScoringAll()
    {
        yield return RunConeScoring(TeamColor.Red, 78,true, true);

    }

    [UnityTest]
    public IEnumerator TestBlueCircuit()
    {
        TestHelper.StartMode(GameMode.Teleop);
        yield return TestConePath(TeamColor.Blue, 36, TestHelper.testPattern);
        //yield return TestConePath(TeamColor.Blue, 52, "A0,B1,B2,C1,D1,D2,D3,D4");
    }

    [UnityTest]
    public IEnumerator TestRedCircuitOnBlue()
    {
        TestHelper.StartMode(GameMode.Teleop);
        yield return TestConePath(TeamColor.Red, 16, TestHelper.testPattern);
    }

    [UnityTest]
    public IEnumerator TestRedCircuit()
    {
        TestHelper.StartMode(GameMode.Teleop);
        yield return TestConePath(TeamColor.Red, 36, "TR0,A4,B3,C2,D1,E0,TR1");
        //yield return TestConePath(TeamColor.Red, 16, testPattern);
    }

    [UnityTest]
    public IEnumerator TestStacking()
    {
        TestHelper.StartMode(GameMode.Teleop);
        yield return TestConePath(TeamColor.Red, 6, "A0,A0,A0");
        
        //yield return TestConePath(TeamColor.Red, 16, testPattern);
    }

    [UnityTest]
    public IEnumerator ScoringConesEndOfGame()
    {
        TestHelper.StartMode(GameMode.Autonomous);
        yield return TestConePath(TeamColor.Blue, 76, "TB0,A0,A1,B0,C0,D0,E0,E1,E2,E3,E4,TB1");
        TestHelper.StopMode();
        //yield return TestConePath(TeamColor.Red, 16, testPattern);
    }

    public IEnumerator TestBotSpawn()
    {
        TestHelper.botOptions = GameObject.FindObjectOfType<SelectBotOptions>();
        TestHelper.botOptions.StartGame();
        DriveReceiverSpinningWheels wheels = TestHelper.botOptions.spawnedBot.GetComponent<DriveReceiverSpinningWheels>();
        yield return new WaitForSeconds(0.5f);
        TestHelper.botOptions.gameObject.SetActive(false);
        for (int i = 0; i < 1209; i++)
        {
            wheels.frontLeft.driveAmount.x = 1;
            wheels.frontRight.driveAmount.x = 1;
            yield return new WaitForEndOfFrame();
        }
    }

    public IEnumerator RunConeScoring(TeamColor color, int correctScore, bool testGround=false,bool testPoles=true)
    {
        yield return new WaitForSeconds(0.5f);
        DropCone cone = GameObject.FindObjectOfType<DropCone>();
        cone.HeightOffset = 1f;
        cone.color = color; 
        
        foreach(JunctionCapper loc in TestHelper.scoringLocs)
        {
            string scoreObjName = loc.transform.parent.parent.parent.name;
            if (scoreObjName.Contains("Ground") && !testGround) { continue; }
            if (!scoreObjName.Contains("Ground") && !testPoles) { continue; }
            cone.Drop(loc.gameObject);
            yield return new WaitForSeconds(0.1f);
        }
        yield return new WaitForSeconds(2);
        ScoreTracker score = TestHelper.scores["Red"];
        ScoreTracker otherScore = TestHelper.scores["Blue"];
        if(color == TeamColor.Blue) { score = TestHelper.scores["Blue"]; otherScore = TestHelper.scores["Red"]; }

        Assert.AreEqual(correctScore, score.Score,"Correct Score for Team?");
        Assert.AreEqual(0, otherScore.Score,"Other team shouldnt have any points");
    }



    public IEnumerator TestConePath(TeamColor color, int correctScore, string coords)
    {
        yield return new WaitForSeconds(0.5f);
        DropCone cone = GameObject.FindObjectOfType<DropCone>();
        cone.HeightOffset = 1f;
        cone.color = color;

        string[] coordsList = coords.Split(',');
        foreach(string coord in coordsList)
        {
            GameObject junction = TestHelper.getGoalOnGrid(coord);
            cone.Drop(junction);
            yield return new WaitForSeconds(.1f);
        }
        
        yield return new WaitForSeconds(4);
        TestHelper.StopMode();
        yield return new WaitForSeconds(30);
        ScoreTracker score = TestHelper.scores["Red"];
        ScoreTracker otherScore = TestHelper.scores["Blue"];
        if (color == TeamColor.Blue) { score = TestHelper.scores["Blue"]; otherScore = TestHelper.scores["Red"]; }

        Assert.AreEqual(correctScore, score.Score, "Correct Score for Team?");
        Assert.AreEqual(0, otherScore.Score, "Other team shouldnt have any points");
        Debug.Log("Expected Score:" + correctScore + ", Actual Score:" + score.Score);
    }
}
