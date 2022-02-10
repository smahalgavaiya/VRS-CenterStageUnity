using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Management/Game Event")]
public class GameEvent : ScriptableObject
{
    List<GameEventListener> gameEventListeners;

    private void OnEnable()
    {
        gameEventListeners = new List<GameEventListener>();
    }

    public void RegisterListener(GameEventListener gameEventListener)
    {
        gameEventListeners.Add(gameEventListener);
    }

    public void UnRegisterListener(GameEventListener gameEventListener)
    {
        gameEventListeners.Remove(gameEventListener);
    }
    public void Raise()
    {
        for (int i = gameEventListeners.Count; i > -1; i--)
        {
            gameEventListeners[i].EventHeard.Invoke();
        }
    }
}
