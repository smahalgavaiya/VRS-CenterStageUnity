using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectBotOptions : MonoBehaviour
{
    public List<GameObject> botPrefabs = new List<GameObject>();
    TeamColor color = TeamColor.Blue;
    int selectedBot = 0;

    public List<Transform> spawnPoints = new List<Transform>();
    // Start is called before the first frame update

    public void SelectBot(int index)
    {
        selectedBot = index;
    }

    public void SelectColor(int index)
    {
        color = (TeamColor)index;
    }

    public void StartGame()
    {
        GameObject bot = GameObject.Instantiate(botPrefabs[selectedBot],spawnPoints[(int)color].position, spawnPoints[(int)color].rotation);
        bot.GetComponent<ColorSwitcher>().TeamColor_ = color;
        bot.GetComponent<ColorSwitcher>().SetColor();
        bot.GetComponent<ScoreObjectTypeLink>().LastTouchedTeamColor = color;
    }

}