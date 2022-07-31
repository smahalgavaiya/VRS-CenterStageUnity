using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class ColorSwitcher : MonoBehaviour
{
    [SerializeField] Material[] materials = new Material[2];

    [SerializeField] TeamColor teamColor;

    [SerializeField] private List<GameObject> objectsToRecolor;

    int currentTeamColor;

    public TeamColor TeamColor_ { get => teamColor; set => teamColor = value; }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if ((int)teamColor != currentTeamColor)
            SetColor();
        else return;
    }

    public void SetColor()
    {
        currentTeamColor = (int)teamColor;
        foreach(GameObject gameObject_ in objectsToRecolor)
        {
            gameObject_.GetComponent<Renderer>().material = materials[currentTeamColor];
        }
    }
}
