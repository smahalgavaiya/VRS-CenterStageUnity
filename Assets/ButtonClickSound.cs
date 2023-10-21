using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ButtonClickSound : MonoBehaviour
{
    public AudioClip sound;
    private AudioSource audioSource;
    // Start is called before the first frame update
    void Start()
    {
        Button b = GetComponent<Button>();
        b.onClick.AddListener(() => playSound());
        audioSource =  gameObject.AddComponent<AudioSource>();
        audioSource.clip = sound;
        audioSource.loop = false;
        audioSource.playOnAwake = false;
    }

    public void playSound()
    {
        audioSource.Play();
    }
}
