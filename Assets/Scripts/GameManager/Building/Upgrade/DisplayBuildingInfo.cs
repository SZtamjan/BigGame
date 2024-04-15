using System;
using System.Collections;
using System.Collections.Generic;
using Economy.EconomyActions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DisplayBuildingInfo : MonoBehaviour
{
    public static DisplayBuildingInfo Instance;

    private GameObject selectedBuilding;

    [SerializeField] private Button demoButton;
    [SerializeField] private Button upgradeButton;
    [SerializeField] private Image buildingImage;
    [SerializeField] private TextMeshProUGUI goldDisplay;
    [SerializeField] private TextMeshProUGUI foodDisplay;
    [SerializeField] private TextMeshProUGUI stoneDisplay;
    [SerializeField] private TextMeshProUGUI woodDisplay;
    [SerializeField] private TextMeshProUGUI title;
    [SerializeField] private TextMeshProUGUI desc;

    public Button UpgradeButton
    {
        get => upgradeButton;
    }
    
    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        demoButton.onClick.AddListener(RemoveBuilding);
        upgradeButton.onClick.AddListener(UpgradeBuilding);
        
        gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        title.text = ":)";
        desc.text = ":)";
        goldDisplay.text = ":)";
        stoneDisplay.text = ":)";
        woodDisplay.text = ":)";
        foodDisplay.text = ":)";
    }

    public void FillDataToDisplayOnRightPanel(BuildingsScriptableObjects info, GameObject currBuildingObj)
    {
        selectedBuilding = currBuildingObj;
        BuildingController selectedController = selectedBuilding.GetComponent<BuildingController>();
        
        Debug.Log("Brak image");
        //selectedBuildingImage.sprite = info
        title.text = info.name;
        desc.text = info.desc;

        if (info.buildingLevelsList.Count > selectedController.CurrentLevel + 1)
        {
            goldDisplay.text = info.buildingLevelsList[selectedController.CurrentLevel+1].thisLevelCost.Gold.ToString();
            stoneDisplay.text = info.buildingLevelsList[selectedController.CurrentLevel+1].thisLevelCost.Stone.ToString();
            woodDisplay.text = info.buildingLevelsList[selectedController.CurrentLevel+1].thisLevelCost.Wood.ToString();
            foodDisplay.text = info.buildingLevelsList[selectedController.CurrentLevel+1].thisLevelCost.Food.ToString();
        }
        else
        {
            goldDisplay.text = "Maxed";
            stoneDisplay.text = "Maxed";
            woodDisplay.text = "Maxed";
            foodDisplay.text = "Maxed";
        }
        
    }

    private void RemoveBuilding()
    {
        Building.Instance.RemoveBuilding(selectedBuilding);
    }

    private void UpgradeBuilding()
    {
        BuildingController selectedController = selectedBuilding.GetComponent<BuildingController>();
        
        if (EconomyOperations.Purchase(selectedController.UpgradeCost))
        {
            selectedController.SaveAndChangeStateTo(BuildingStates.StartUpgrade);
        } 
        
        UpgradeButton.interactable = selectedController.CurrentState == BuildingStates.Normal;
        if (selectedController.BuildingMaxed) UpgradeButton.interactable = false;
    }
}
