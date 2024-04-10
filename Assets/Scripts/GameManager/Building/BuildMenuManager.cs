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
        
        Debug.Log("Brak image");
        //selectedBuildingImage.sprite = info
        selectedBuildingTitle.text = info.name;
        selectedBuildingDescription.text = info.desc;
        
        goldDisplay.text = info.buyCost.Gold.ToString();
        stoneDisplay.text = info.buyCost.Stone.ToString();
        woodDisplay.text = info.buyCost.Wood.ToString();
        foodDisplay.text = info.buyCost.Food.ToString();
    }
    
    public void InitBuyBuilding()
    {
        if (EconomyOperations.CheckIfICanIAfford(_buildingInfo.buyCost)) //tu building info zapewne trzeba zmienic na cene z levela
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
