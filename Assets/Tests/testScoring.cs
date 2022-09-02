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
    GameObject[,] gameGrid;
    int gridHeight = 5;
    int gridWidth = 5;
    JunctionCapper[] scoringLocs;
    string testPattern = "A0,B1,C2,D3,E4";

    GameObject getGoalOnGrid(string coords)
    {
        Vector2Int loc = getGridLocation(coords);
        return gameGrid[loc.x, loc.y];
    }

    Vector2Int getGridLocation(string coords)
    {
        int acode = (int)'A';
        int column = (int)coords[0];
        column -= acode;
        return new Vector2Int(column, int.Parse(coords[1].ToString()));
    }

    void setGoalOnGrid(string coords, GameObject obj)
    {
        Vector2Int loc = getGridLocation(coords);
        gameGrid[loc.x,loc.y] = obj;
    }


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
        gameGrid = new GameObject[gridWidth,gridHeight];
        SceneManager.LoadScene("PowerPlayNewBots",LoadSceneMode.Single);
    }

    public void CreateGrid()
    {
        scoringLocs = GameObject.FindObjectsOfType<JunctionCapper>();
        foreach(JunctionCapper cap in scoringLocs)
        {
            string coords = cap.transform.parent.parent.parent.name.Split('-')[1];//need a 
            setGoalOnGrid(coords, cap.gameObject);
        }
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

    [UnityTest]
    public IEnumerator TestBlueCircuit()
    {
        yield return TestConePath(TeamColor.Blue, 34, testPattern);
        //yield return TestConePath(TeamColor.Blue, 52, "A0,B1,B2,C1,D1,D2,D3,D4");
        //yield return TestConePath(TeamColor.Red, 16, testPattern);
    }

    [UnityTest]
    public IEnumerator TestRedCircuitOnBlue()
    {
        yield return TestConePath(TeamColor.Red, 14, testPattern);
        //yield return TestConePath(TeamColor.Red, 16, testPattern);
    }

    [UnityTest]
    public IEnumerator TestRedCircuit()
    {
        yield return TestConePath(TeamColor.Red, 34, "A4,B3,C2,D1,E0");
        //yield return TestConePath(TeamColor.Red, 16, testPattern);
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
        ReadyTest();
        yield return new WaitForSeconds(0.5f);
        DropCone cone = GameObject.FindObjectOfType<DropCone>();
        cone.HeightOffset = 1f;
        cone.color = color; 
        
        foreach(JunctionCapper loc in scoringLocs)
        {
            string scoreObjName = loc.transform.parent.parent.parent.name;
            if (scoreObjName.Contains("Ground") && !testGround) { continue; }
            if (!scoreObjName.Contains("Ground") && !testPoles) { continue; }
            cone.Drop(loc.gameObject);
            yield return new WaitForSeconds(0.1f);
        }
        yield return new WaitForSeconds(2);
        ScoreTracker score = scores["Red"];
        ScoreTracker otherScore = scores["Blue"];
        if(color == TeamColor.Blue) { score = scores["Blue"]; otherScore = scores["Red"]; }

        Assert.AreEqual(correctScore, score.Score,"Correct Score for Team?");
        Assert.AreEqual(0, otherScore.Score,"Other team shouldnt have any points");
    }

    public void ReadyTest()//this needs to be called after scene is ready, which is why its not in setup.
    {
        CreateGrid();
        SelectBotOptions botgui = GameObject.FindObjectOfType<SelectBotOptions>();
        if (botgui) { botgui.gameObject.SetActive(false); }
        
    }

    public IEnumerator TestConePath(TeamColor color, int correctScore, string coords)
    {
        ReadyTest();
        yield return new WaitForSeconds(0.5f);
        DropCone cone = GameObject.FindObjectOfType<DropCone>();
        cone.HeightOffset = 1f;
        cone.color = color;

        string[] coordsList = coords.Split(',');
        foreach(string coord in coordsList)
        {
            GameObject junction = getGoalOnGrid(coord);
            cone.Drop(junction);
            yield return new WaitForSeconds(.1f);
        }

        yield return new WaitForSeconds(2);
        ScoreTracker score = scores["Red"];
        ScoreTracker otherScore = scores["Blue"];
        if (color == TeamColor.Blue) { score = scores["Blue"]; otherScore = scores["Red"]; }

        Assert.AreEqual(correctScore, score.Score, "Correct Score for Team?");
        Assert.AreEqual(0, otherScore.Score, "Other team shouldnt have any points");
    }
}
