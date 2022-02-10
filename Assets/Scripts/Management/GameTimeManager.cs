using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameTimeManager : MonoBehaviour
{
    public UnityEvent gameStarts;
    public GlobalInt gameTime;

    // Start is called before the first frame update
    void Start()
    {
        gameStarts.Invoke();
    }

    // Update is called once per frame
    void Update()
    {
        gameTime.globalInt = (int)Time.realtimeSinceStartup;
        
    }
}
