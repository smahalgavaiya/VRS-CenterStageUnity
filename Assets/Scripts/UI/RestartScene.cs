using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartScene : MonoBehaviour
{
    public void Restart()
    {
        Debug.Log("restart");
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
