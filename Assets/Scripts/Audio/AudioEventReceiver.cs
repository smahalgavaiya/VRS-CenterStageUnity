using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(GameEventListener))]
public class AudioEventReceiver : MonoBehaviour
{
    public AudioClip[] audioClips;
    AudioSource audioSource;

    public void PlayAudioClip()
    {
        audioSource.clip = audioClips[0];
        audioSource.Play();
    }
    public void PlayAudioClip(int index)
    {
        audioSource.clip = audioClips[index];
        audioSource.Play();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
