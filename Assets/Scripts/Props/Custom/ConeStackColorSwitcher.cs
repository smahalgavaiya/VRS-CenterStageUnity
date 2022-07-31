using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class ConeStackColorSwitcher : MonoBehaviour
{
    [SerializeField] TeamColor teamColor;
    List<ColorSwitcher> coneColorSwitchers;

    int currentTeamColor;
    // Start is called before the first frame update
    void Start()
    {
        GetConeColorSwitchers();
    }

    void GetConeColorSwitchers()
    {
        currentTeamColor = (int)teamColor;
        if (coneColorSwitchers == null)
            coneColorSwitchers = new List<ColorSwitcher>();
        coneColorSwitchers.Clear();

        for (int i = 0; i < transform.childCount; i++)
        {
            coneColorSwitchers.Add(transform.GetChild(i).gameObject.GetComponent<ColorSwitcher>());
        }
    }

    void ChangeConeColors()
    {
        foreach (ColorSwitcher colorSwitcher in coneColorSwitchers)
        {
            colorSwitcher.TeamColor_ = teamColor;
            colorSwitcher.SetColor();
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (coneColorSwitchers.Count < 10)
            GetConeColorSwitchers();
        
        if ((int)teamColor != currentTeamColor)
        {
            ChangeConeColors();
            currentTeamColor = (int)teamColor;
        }
    }
}
