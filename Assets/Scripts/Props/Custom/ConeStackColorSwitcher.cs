using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class ConeStackColorSwitcher : MonoBehaviour
{
    [SerializeField] TeamColor teamColor;
    List<ColorSwitcher> coneColorSwitchers;
    List<ScoreObjectTypeLink> scoreObjectTypeLinks;

    int currentTeamColor;
    // Start is called before the first frame update
    void Start()
    {
        GetChildObjects();
    }

    public void GetChildObjects()
    {
        currentTeamColor = (int)teamColor;
        if (coneColorSwitchers == null)
            coneColorSwitchers = new List<ColorSwitcher>();
        coneColorSwitchers.Clear();

        if (scoreObjectTypeLinks == null)
            scoreObjectTypeLinks = new List<ScoreObjectTypeLink>();
        scoreObjectTypeLinks.Clear();

        for (int i = 0; i < transform.childCount; i++)
        {
            coneColorSwitchers.Add(transform.GetChild(i).gameObject.GetComponent<ColorSwitcher>());
            scoreObjectTypeLinks.Add(transform.GetChild(i).gameObject.GetComponent<ScoreObjectTypeLink>());
        }
    }

    void ChangeConeColors()
    {
        GetChildObjects();
        foreach (ColorSwitcher colorSwitcher in coneColorSwitchers)
        {
            colorSwitcher.TeamColor_ = teamColor;
            colorSwitcher.SetColor();
        }

        foreach (ScoreObjectTypeLink scoreObjectTypeLink in scoreObjectTypeLinks)
        {
            scoreObjectTypeLink.LastTouchedTeamColor = teamColor;
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (coneColorSwitchers.Count < 5)
            GetChildObjects();
        
        if ((int)teamColor != currentTeamColor)
        {
            ChangeConeColors();
            currentTeamColor = (int)teamColor;
        }
    }
}
