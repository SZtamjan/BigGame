using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private GameObject exitWindow;
    [SerializeField] private GameObject settings;
    [SerializeField] private GameObject audioSettings;
    [SerializeField] private GameObject menu;

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


    


}
