using System;
using System.Collections;
using System.Collections.Generic;
using Economy.EconomyActions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuildMenuManager : MonoBehaviour
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
    
    public void FillDataToDisplayOnRightPanel(BuildingsScriptableObjects info)
    {
        _buildingInfo = info;

        if(info.buildingImage == null)
        {
            Debug.LogWarning("Brak image budynku");
        }
        else
        {
            selectedBuildingImage.sprite = info.buildingImage;
        }
        selectedBuildingTitle.text = info.name;
        selectedBuildingDescription.text = info.desc;
        
        goldDisplay.text = info.buildingLevelsList[0].thisLevelCost.Gold.ToString();
        stoneDisplay.text = info.buildingLevelsList[0].thisLevelCost.Stone.ToString();
        woodDisplay.text = info.buildingLevelsList[0].thisLevelCost.Wood.ToString();
        foodDisplay.text = info.buildingLevelsList[0].thisLevelCost.Food.ToString();
    }
    
    public void InitBuyBuilding()
    {
        if (EconomyOperations.CheckIfICanIAfford(_buildingInfo.buildingLevelsList[0].thisLevelCost))
        {
            StartBulding();
        }
    }
    
    public void StartBulding()
    {
        UIController.Instance.BuildingCardsChangeShow(false);
        Building.Instance.StartBuilding(_buildingInfo);
    }
}
