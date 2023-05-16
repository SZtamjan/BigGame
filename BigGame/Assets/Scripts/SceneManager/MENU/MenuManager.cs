using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class MenuManager : MonoBehaviour
{
    public static MenuManager instance;
    [SerializeField] private GameObject exitWindow;
    [SerializeField] private GameObject settings;
    [SerializeField] private GameObject audioSettings;
    [SerializeField] private GameObject menu;
    [SerializeField] private Image sfx;
    [SerializeField] private Sprite sex;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        
        RandomEaster();
    }

    #region Menu
    public void ShowMenu()
    {
        exitWindow.SetActive(false);
        settings.SetActive(false);
        menu.SetActive(true);
    }


    #endregion

    #region Settings
    public void ShowSettings()
    {
        menu.SetActive(false);
        audioSettings.SetActive(false);
        settings.SetActive(true);
    }

    public void ShowAudioSettings()
    {
        settings.SetActive(false);
        audioSettings.SetActive(true);
    }

    #endregion
    #region QuitingGame
    public void QuitGame()
    {
        Application.Quit();
    }

    public void OpenQuitGameWindow()
    {
        exitWindow.SetActive(true);
    }

    public void CloseQuitGameWindow()
    {
        exitWindow.SetActive(false);
    }

    #endregion

    #region jajoWielkanocne

    public void RandomEaster()
    {
        int rnd = Random.Range(0, 500);
        if (rnd == 250)
        {
            sfx.sprite = sex;
        }
    }

    #endregion
}