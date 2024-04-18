using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DetectHoover : MonoBehaviour
{
    [SerializeField] private GameObject popUpWindow;
    
    public void ActivateWindow()
    {
        popUpWindow.SetActive(true);
    }

    public void DisableWindow()
    {
        popUpWindow.SetActive(false);
    }
}
