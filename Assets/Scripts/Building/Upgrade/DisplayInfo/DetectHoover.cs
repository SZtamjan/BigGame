using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class DetectHoover : MonoBehaviour
{
    [SerializeField] private GameObject floatingWindow;
    [SerializeField] private Button upgradeBtn;
    
    [Header("Stats")] 
    [SerializeField] private DisplayBuildingInfo infoScript;

    [SerializeField] private InfoTypeToSend whatToSend;

    //Components
    private FloatingWindowDisplayBuildingInfo _floatingWindowDisplayBuildingInfo; 

    private void Start()
    {
        _floatingWindowDisplayBuildingInfo = floatingWindow.GetComponent<FloatingWindowDisplayBuildingInfo>();
    }

    public void ActivateWindow()
    {
        SendDataToWindow();
        floatingWindow.SetActive(true);
    }

    public void DisableWindow()
    {
        floatingWindow.SetActive(false);
    }

    private void SendDataToWindow()
    {
        BuildingController bCon = infoScript.SelectedBuilding.GetComponent<BuildingController>();
        
        switch (whatToSend)
        {
            case InfoTypeToSend.UpgradeValue:
                _floatingWindowDisplayBuildingInfo.FillData(bCon.UpgradeCost,bCon.TurnOffForTurns);
                break;
            case InfoTypeToSend.GainValue:
                _floatingWindowDisplayBuildingInfo.FillData(bCon.ResourcesCurrentMaxGain);
                Debug.LogWarning("Probably won't display as intended, not really implemented");
                break;
            case InfoTypeToSend.SellValue:
                _floatingWindowDisplayBuildingInfo.FillData(bCon.ResourcesCurrentSell);
                break;
            default:
                Debug.LogWarning("Exception :(");
                break;
        }
    }
}

public enum InfoTypeToSend
{
    UpgradeValue,
    GainValue,
    SellValue
}