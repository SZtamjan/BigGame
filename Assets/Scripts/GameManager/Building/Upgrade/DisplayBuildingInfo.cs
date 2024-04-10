using System;
using System.Collections;
using System.Collections.Generic;
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
        
        Debug.Log("Brak image");
        //selectedBuildingImage.sprite = info
        title.text = info.name;
        desc.text = info.desc;
        
        goldDisplay.text = info.buyCost.Gold.ToString();
        stoneDisplay.text = info.buyCost.Stone.ToString();
        woodDisplay.text = info.buyCost.Wood.ToString();
        foodDisplay.text = info.buyCost.Food.ToString();
    }

    private void RemoveBuilding()
    {
        Building.Instance.RemoveBuilding(selectedBuilding);
    }

    private void UpgradeBuilding()
    {
        selectedBuilding.GetComponent<BuildingController>().SaveAndChangeStateTo(BuildingStates.StartUpgrade);
        UpgradeButton.interactable = selectedBuilding.GetComponent<BuildingController>().CurrentState == BuildingStates.Normal;
        if (selectedBuilding.GetComponent<BuildingController>().BuildingMaxed) UpgradeButton.interactable = false;
    }
}
