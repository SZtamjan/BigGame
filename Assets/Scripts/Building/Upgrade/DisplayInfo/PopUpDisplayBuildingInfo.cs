using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class PopUpDisplayBuildingInfo : MonoBehaviour
{
    //Components
    private UIController _uiController;
    
    [Header("Move window")] 
    [SerializeField] private Camera uiCam;
    [SerializeField] private RectTransform objToMove;
    [SerializeField] private Vector2 objectOffset;

    [Header("Stats")] 
    [SerializeField] private DisplayBuildingInfo infoScript; 
    [SerializeField] private TextMeshProUGUI gold;
    [SerializeField] private TextMeshProUGUI food;
    [SerializeField] private TextMeshProUGUI wood;
    [SerializeField] private TextMeshProUGUI stone;

    private void Awake()
    {
        _uiController = UIController.Instance;
    }

    private void Update()
    {
        MoveObjectWithBuildingInfo();
    }

    private void MoveObjectWithBuildingInfo()
    {
        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(objToMove.parent as RectTransform, Input.mousePosition, uiCam, out localPoint);
        localPoint += objectOffset;
        objToMove.localPosition = localPoint;
    }

    private void OnEnable()
    {
        BuildingController bCon = infoScript.SelectedBuilding.GetComponent<BuildingController>();
        FillData(bCon.UpgradeCost, bCon.TurnOffForTurns);
    }

    private void FillData(ResourcesStruct res, int turnedOffForTurns)
    {
        gold.text = res.Gold.ToString();
        food.text = res.Food.ToString();
        wood.text = res.Wood.ToString();
        stone.text = res.Stone.ToString();
        
        _uiController.DisplayForHowLongIsDisabled(turnedOffForTurns);
    }
}
