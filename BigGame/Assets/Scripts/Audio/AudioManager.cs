using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
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

    private void UpdateSoundLevel()
    {
        GetComponent<MixerData>().myMixer.SetFloat("masterMixer", Mathf.Log10(PlayerPrefs.GetFloat("masterAudio")) * 20);
        GetComponent<MixerData>().myMixer.SetFloat("musicMixer", Mathf.Log10(PlayerPrefs.GetFloat("music")) * 20);
        GetComponent<MixerData>().myMixer.SetFloat("sfxMixer", Mathf.Log10(PlayerPrefs.GetFloat("sfx")) * 20);
    }
    
    public void KorutynaCzas()
    {
        UpdateSoundLevel();
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
