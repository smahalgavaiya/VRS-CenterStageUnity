using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;

public class DisplayLeaderboard : MonoBehaviour
{
    private vrs_messenger messenger;
    private List<scoreEntry> entries;
    private TextMeshProUGUI text;

    public GameObject rowPrefab;
    // Start is called before the first frame update
    private void OnEnable()
    {
        clearChildren(transform);
        entries = new List<scoreEntry>();
        //entries.Add(new scoreEntry { user_name = "ChrisAsh", points = 20, date = "11-25-81" });

        messenger = FindFirstObjectByType<vrs_messenger>(); 
        
        text = GetComponentInChildren<TextMeshProUGUI>();
        Debug.Log(entries);
        StartCoroutine(refreshProcess());
    }

    private IEnumerator refreshProcess()
    {
        
        messenger.RequestLeaderboard();
        while(messenger.leaderboard == null || messenger.leaderboard.Length == 0)
        {
            yield return new WaitForSecondsRealtime(0.25f);
        }
        Refresh();
    }

    private void Refresh()
    {
        string output = "";
        //output += $"USER - POINTS - DATE\n";
        Instantiate(rowPrefab, transform);
        foreach (scoreEntry entry in messenger.leaderboard/*entries*/)
        {
            //transfer to sql query for game mode
            if(entry.game != SimManager.CurrentCourse.shortname)
            {
                continue;
            }
            GameObject row = Instantiate(rowPrefab, transform);
            row = row.transform.Find("Data").gameObject;
            TextMeshProUGUI rowtext = row.transform.Find("Name").gameObject.GetComponentInChildren<TextMeshProUGUI>();
            rowtext.text = entry.user_name;
            rowtext = row.transform.Find("Points").gameObject.GetComponentInChildren<TextMeshProUGUI>();
            rowtext.text = entry.points.ToString();
            rowtext = row.transform.Find("Date").gameObject.GetComponentInChildren<TextMeshProUGUI>();
            rowtext.text = entry.date;
            //output += $"{entry.user_id} - {entry.points} - {entry.date}\n";
        }
        //text.text = output;
        //text.text = messenger.scoreTest;
    }

    public static void clearChildren(Transform t, string exception = "none", bool immediate = false)
    {
        List<GameObject> objs = new List<GameObject>();
        foreach (Transform child in t)
        {
            if (child.name != exception)
            {
                objs.Add(child.gameObject);
            }

        }
        foreach (GameObject c in objs)
        {
            if (immediate)
            {
                Object.DestroyImmediate(c);
            }
            else
            {
                Object.Destroy(c);
            }
        }
    }


}
