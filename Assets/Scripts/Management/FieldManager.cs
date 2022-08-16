using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameMode
{
    Autonomous,
    Teleop
}
public class FieldManager : MonoBehaviour
{
    public GameMode mode;

    private static FieldManager _instance;
    public static FieldManager instance
    {
        get {return _instance;}
    }

    void Awake()
    {
        if(_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }
    void Start()
    {}
    void Update()
    {}

    public void SetMode(int gameMode)
    {
        mode = (GameMode)gameMode;
    }

}
