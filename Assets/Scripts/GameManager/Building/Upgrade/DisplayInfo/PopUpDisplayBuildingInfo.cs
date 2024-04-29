using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class PopUpDisplayBuildingInfo : MonoBehaviour
{
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

    private void Start()
    {
        gameObject.SetActive(false);
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
        FillData(infoScript.SelectedBuilding.GetComponent<BuildingController>().UpgradeCost);
    }

    private void FillData(ResourcesStruct res)
    {
        gold.text = res.Gold.ToString();
        food.text = res.Food.ToString();
        wood.text = res.Wood.ToString();
        stone.text = res.Stone.ToString();
    }
}
