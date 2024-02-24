using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioCategory : MonoBehaviour
{
    //Audio
    [Header("Audio")]
    public AudioSource menuSong;
    public AudioSource sfx;

    public void PlayIdleSong()
    {
        menuSong.GetComponent<AudioSource>().Play();
    }
}
