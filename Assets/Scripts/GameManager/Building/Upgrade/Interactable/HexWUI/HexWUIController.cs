using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HexWUIController : MonoBehaviour
{
    private GameObject hexWUI;
    private GameObject hexWUIRoratable;
    
    private TextMeshProUGUI gold;
    private TextMeshProUGUI food;
    private TextMeshProUGUI wood;
    private TextMeshProUGUI stone;

    private InteractableHex _interactableHex;

    #region Start & SetUp

    void Start()
    {
        FindMyWUI();
        PrepareMyInfo();
        PrepareForWork();
    }

    private void FindMyWUI()
    {
        Transform parent = transform.parent;
        foreach (Transform child in parent)
        {
            if (child.gameObject.CompareTag("HexWUI"))
            {
                hexWUI = child.gameObject;
                break;
            }
        }

        HexWUIInfoHolder hexWuiInfoHolder = hexWUI.GetComponent<HexWUIInfoHolder>();
        
        hexWUIRoratable = hexWuiInfoHolder.HexWuiRoratable;
        
        gold = hexWuiInfoHolder.Gold;
        food = hexWuiInfoHolder.Food;
        wood = hexWuiInfoHolder.Wood;
        stone = hexWuiInfoHolder.Stone;
    }

    private void PrepareMyInfo()
    {
        _interactableHex = GetComponent<InteractableHex>();
    }

    private void PrepareForWork()
    {
        hexWUI.SetActive(false);
    }

    #endregion

    private void OnMouseEnter()
    {
        PrepareMyInfoToDisplay();
        ChangeVisibility(true);
    }
    
    public void OnMouseExit()
    {
        ChangeVisibility(false);
    }

    private void PrepareMyInfoToDisplay()
    {
        if(gold != null) gold.text = _interactableHex.HexResources.Gold.ToString();
        if(food != null) food.text = _interactableHex.HexResources.Food.ToString();
        if(wood != null) wood.text = _interactableHex.HexResources.Wood.ToString();
        if(stone != null) stone.text = _interactableHex.HexResources.Stone.ToString();
    }

    private void ChangeVisibility(bool isVisible)
    {
        hexWUI.SetActive(isVisible);
    }
}
