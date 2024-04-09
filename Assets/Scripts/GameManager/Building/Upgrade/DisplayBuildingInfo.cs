using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class DisplayBuildingInfo : MonoBehaviour
{
    public static DisplayBuildingInfo Instance;

    private GameObject selectedBuilding;
    
    [SerializeField] private Image buildingImage;
    [SerializeField] private TextMeshProUGUI goldDisplay;
    [SerializeField] private TextMeshProUGUI foodDisplay;
    [SerializeField] private TextMeshProUGUI stoneDisplay;
    [SerializeField] private TextMeshProUGUI woodDisplay;
    [SerializeField] private TextMeshProUGUI title;
    [SerializeField] private TextMeshProUGUI desc;
    
    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
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

        Debug.LogWarning("Zhardkodowane lvl 0"); //na pewno? xd
        goldDisplay.text = info.buyCost.Gold.ToString();
        stoneDisplay.text = info.buyCost.Stone.ToString();
        woodDisplay.text = info.buyCost.Wood.ToString();
        foodDisplay.text = info.buyCost.Food.ToString();
    }
}
