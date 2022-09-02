using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SelectBotOptions : MonoBehaviour
{
    public List<GameObject> botPrefabs = new List<GameObject>();
    TeamColor color = TeamColor.Blue;
    int selectedBot = 0;
    bool useLowerSpawn = false;
    bool preloadCone = false;

    public List<Transform> spawnPoints = new List<Transform>();
    public GameObject ConePrefab;

    public UnityEvent FinishedStart;
    public GameObject spawnedBot;
    // Start is called before the first frame update

    public void SelectBot(int index)
    {
        selectedBot = index;
    }

    public void SetSpawn(bool useLower)
    {
        useLowerSpawn = useLower;
    }

    public void SetCone(bool preload)
    {
        preloadCone = preload;
    }

    public void SelectColor(int index)
    {
        color = (TeamColor)index;
    }

    public void StartGame()
    {
        if (spawnedBot) { Destroy(spawnedBot); }
        int spawnIdx = (int)color;
        if (useLowerSpawn) { spawnIdx += 2; }
        GameObject bot = GameObject.Instantiate(botPrefabs[selectedBot],spawnPoints[spawnIdx].position, spawnPoints[spawnIdx].rotation);
        bot.GetComponent<ColorSwitcher>().TeamColor_ = color;
        bot.GetComponent<ColorSwitcher>().SetColor();
        bot.GetComponent<ScoreObjectTypeLink>().LastTouchedTeamColor = color;
        spawnedBot = bot;
        FieldManager.botColor = color;
        if(preloadCone)
        {
            StartCoroutine(DoPreload(bot));
        }
        else { FinishedStart.Invoke(); }
    }

    IEnumerator DoPreload(GameObject bot)
    {
        GameObject cone = GameObject.Instantiate(ConePrefab, bot.transform.position, bot.transform.rotation);
        cone.GetComponent<ColorSwitcher>().TeamColor_ = color;
        cone.GetComponent<ColorSwitcher>().SetColor();
        cone.GetComponent<ScoreObjectTypeLink>().LastTouchedTeamColor = color;
        ObjectGrabber grabber = bot.GetComponentInChildren<ObjectGrabber>();
        ObjectChecker checker = grabber.GetComponent<ObjectChecker>();

        checker.CanPickUp = true;
        cone.transform.position = grabber.transform.position;
        yield return new WaitForFixedUpdate();
        grabber.PickUpOrPutDownObject();
        FinishedStart.Invoke();
    }
}