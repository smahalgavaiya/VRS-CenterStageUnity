using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameEventListener : MonoBehaviour
{
    public GameEvent gameEvent;
    public UnityEvent EventHeard;

    private void Awake()
    {
        gameEvent.RegisterListener(this);
    }
    private void OnDestroy()
    {
        gameEvent.UnRegisterListener(this);
    }
}
