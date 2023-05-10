using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [Header("Main Obiects")]
    public AudioSource musicAS;
    public AudioSource sfxAS;

    private int previousIndex;
    private bool firstTime = false;

    [Header("Sounds Lists")]
    public List<AudioClip> music;
    public List<AudioClip> soundEffectd;

    void Awake()
    {
        instance = this;
    }

    public void KorutynaCzas()
    {
        StartCoroutine(CheckForSongOver());
    }

    public void PlaySFX(int i)
    {
        sfxAS.clip = soundEffectd[i];
        sfxAS.Play();
    }

    public IEnumerator CheckForSongOver()
    {
        PlayIdleSong();
        while (true)
        {
            if(!musicAS.isPlaying) PlayIdleSong();
            yield return null;
        }
    }
    
    public void PlayIdleSong()
    {
        int i = RandomTrack();
        musicAS.clip = music[i];
        musicAS.Play();
    }
    private int RandomTrack()
    {
        int i;
        do
        {
            i = Random.Range(0, music.Count);
        } while (i == previousIndex);
        
        if (!firstTime)
        {
            i = 0;
            firstTime = true;
        }
        previousIndex = i;
        return i;
    }
}
