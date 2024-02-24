using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveAudioVolume : MonoBehaviour
{
    public void SaveMusicVolume(float musicVolume)
    {
        PlayerPrefs.SetFloat("music", musicVolume);
    }
    public void SaveSFXVolume(float sfxVolume)
    {
        PlayerPrefs.SetFloat("sfx", sfxVolume);
    }
}
