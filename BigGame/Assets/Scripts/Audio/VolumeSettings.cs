using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeSettings : MonoBehaviour
{
    [SerializeField] private AudioMixer myMixer;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;

    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.HasKey("music"))
        {
            musicSlider.value = PlayerPrefs.GetFloat("music");
        }
        else
        {
            PlayerPrefs.SetFloat("music", musicSlider.value);
        }

        if (PlayerPrefs.HasKey("sfx"))
        {
            sfxSlider.value = PlayerPrefs.GetFloat("sfx");
        }
        else
        {
            PlayerPrefs.SetFloat("sfx", sfxSlider.value);
        }

        if (PlayerPrefs.HasKey("masterAudio"))
        {
            PlayerPrefs.SetFloat("masterAudio", 0f);
        }
        else
        {
            PlayerPrefs.SetFloat("masterAudio", 0f);
        }

        SetMusicVolume();
        SetSFXVolume();
    }

    public void SetMusicVolume()
    {
        float musicVolume = musicSlider.value;
        
        myMixer.SetFloat("musicMixer", Mathf.Log10(musicVolume) * 20);
        
        gameObject.GetComponent<SaveAudioVolume>().SaveMusicVolume(musicVolume);
    }

    public void SetSFXVolume()
    {
        float sfxVolume = sfxSlider.value;
        myMixer.SetFloat("sfxMixer", Mathf.Log10(sfxVolume) * 20);
        gameObject.GetComponent<SaveAudioVolume>().SaveSFXVolume(sfxVolume);
    }
}
