using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    public GameObject music;

    public void Awake()
    {
        instance = this;
    }

    public void PlayIdleSong()
    {
        music.GetComponent<AudioSource>().Play();
    }


}
