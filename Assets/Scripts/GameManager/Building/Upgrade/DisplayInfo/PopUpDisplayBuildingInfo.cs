using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class PopUpDisplayBuildingInfo : MonoBehaviour
{
    [Header("Move window")]
    [SerializeField] private Camera mainCam;
    [SerializeField] private RectTransform objToMove;
    [SerializeField] private RectTransform parent;

    [Header("Stats")] 
    [SerializeField] private DisplayBuildingInfo infoScript; 
    [SerializeField] private TextMeshProUGUI gold;
    [SerializeField] private TextMeshProUGUI food;
    [SerializeField] private TextMeshProUGUI wood;
    [SerializeField] private TextMeshProUGUI stone;
    private void Update()
    {
        MoveObjectWithBuildingInfo();
    }

    private void MoveObjectWithBuildingInfo()
    {
        Vector3 pos = Input.mousePosition;
        pos.z = parent.position.z;
        objToMove.position = mainCam.ScreenToWorldPoint(pos);
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
