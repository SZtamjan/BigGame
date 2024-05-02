using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DetectHoover : MonoBehaviour
{
    [SerializeField] private GameObject popUpWindow;
    [SerializeField] private Button upgradeBtn;

    public void ActivateWindow()
    {
        if (upgradeBtn.interactable) //if button is disabled, then dont show popup window
        {
            popUpWindow.SetActive(true);
        }
    }

    public void DisableWindow()
    {
        popUpWindow.SetActive(false);
    }
}
