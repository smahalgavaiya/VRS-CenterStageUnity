using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class ColorSwitcher : MonoBehaviour
{
    [SerializeField] Material[] materials = new Material[2];

    [SerializeField] TeamColor teamColor;

    [SerializeField] List<GameObject> objectsToRecolor;

    int currentTeamColor;
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

    void SetColor()
    {
        currentTeamColor = (int)teamColor;
        foreach(GameObject gameObject_ in objectsToRecolor)
        {
            gameObject_.GetComponent<Renderer>().material = materials[currentTeamColor];
        }
    }
}
