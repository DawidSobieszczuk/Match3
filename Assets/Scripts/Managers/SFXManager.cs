using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXManager : Manager<SFXManager>
{
    AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void Play(AudioClip audioClip)
    {
        if(PlayerPrefs.GetInt("SFX", 1) != 0)
        {
            audioSource.clip = audioClip;
            audioSource.Play();
        }
    }
}
