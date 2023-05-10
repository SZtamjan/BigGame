using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using UnityEngine;
using Image = UnityEngine.UI.Image;

public class MuteSwitch : MonoBehaviour
{
    public Sprite isOn;
    public Sprite isOff;
    public GameObject mixer;

    private float currentVol;
    private void Start()
    {
        SwitchMute();
    }

    public void SwitchMute()
    {
        if (GetComponent<Image>().sprite == isOn)
        {
            Mute();
            Debug.Log("Muted");
            GetComponent<Image>().sprite = isOff;
        }
        else
        {
            Unmute();
            Debug.Log("Unmuted");
            GetComponent<Image>().sprite = isOn;
        }
    }
    
    private void Mute()
    {
        mixer.GetComponent<MixerData>().myMixer.SetFloat("masterMixer", -80);
    }

    private void Unmute()
    {
        mixer.GetComponent<MixerData>().myMixer.SetFloat("masterMixer", PlayerPrefs.GetFloat("masterAudio"));
    }
    
}
