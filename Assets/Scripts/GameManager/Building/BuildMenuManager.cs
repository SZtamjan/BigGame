using System;
using System.Collections;
using System.Collections.Generic;
using Economy.EconomyActions;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class BuildMenuManager : EconomyOperations
{
    public static BuildMenuManager Instance;
    
    [Header("Display Info About Building")]
    [SerializeField] private Image selectedBuildingImage;
    [SerializeField] private TextMeshProUGUI selectedBuildingTitle;
    [SerializeField] private TextMeshProUGUI selectedBuildingDescription;
    [SerializeField] private TextMeshProUGUI goldDisplay;
    [SerializeField] private TextMeshProUGUI stoneDisplay;
    [SerializeField] private TextMeshProUGUI woodDisplay;
    [SerializeField] private TextMeshProUGUI foodDisplay;
    
    [Header("")]
    [SerializeField] private Button button;
    
    private BuildingsScriptableObjects _buildingInfo;
    private Coroutine checkingInProgress;

    private void Awake()
    {
        Instance = this;

        button.interactable = false;
    }

    private void OnEnable()
    {
        //_buildingInfo = null;
        if (_buildingInfo == null)
        {
            checkingInProgress ??= StartCoroutine(CheckIfBuildingInfoFilled());
        }
        else
        {
            //FillData(_buildingInfo); //Dont need it
        }
    }

    private void OnDisable()
    {
        if(checkingInProgress != null) StopCoroutine(checkingInProgress);
        checkingInProgress = null;
    }

    private IEnumerator CheckIfBuildingInfoFilled()
    {
        while (_buildingInfo == null)
        {
            yield return null;
        }

        button.interactable = true;
        
        checkingInProgress = null;
    }
    
    public void FillData(BuildingsScriptableObjects info)
    {
        _buildingInfo = info;
        
        Debug.Log("Brak image");
        //selectedBuildingImage.sprite = info
        selectedBuildingTitle.text = info.name;
        selectedBuildingDescription.text = info.desc;

        goldDisplay.text = info.resourcesCost.Gold.ToString();
        stoneDisplay.text = info.resourcesCost.Stone.ToString();
        woodDisplay.text = info.resourcesCost.Wood.ToString();
        foodDisplay.text = info.resourcesCost.Food.ToString();
    }
    
    public void InitBuyBuilding()
    {
        // bool CanIBuy = EconomyResources.Instance.CanIBuy(_buildingInfo.cost);
        // if(CanIBuy)
        // {
        //     StartBulding();
        // }
        if (CheckIfICanIAfford(_buildingInfo.resourcesCost))
        {
            StartBulding();
        }
    }
    
    public void StartBulding()
    {
        UIController.Instance.BuildingCardsChangeShow(false);
        Building.Instance.StartBuilding(_buildingInfo);
    }

    public void StartDemolition()
    {
        DestroyBuilding.Instance.StartDestroying();
    }
    
}
