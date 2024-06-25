using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class FloatingWindowDisplayBuildingInfo : MonoBehaviour
{
    //Components
    private UIController _uiController;
    
    [Header("Move window")] 
    [SerializeField] private Camera uiCam;
    [SerializeField] private RectTransform objToMove;
    [SerializeField] private Vector2 objectOffset;
    
    [Header("Titles")]
    [SerializeField] private string titleForUpgrade = "Upgrade";
    [SerializeField] private string titleForGain = "coooo proszeee";
    [SerializeField] private string titleForSell = "Sell";

    private void Start()
    {
        _uiController = UIController.Instance;
        gameObject.SetActive(false);
    }

    private void Update()
    {
        MoveObjectWithBuildingInfo();
    }

    private void MoveObjectWithBuildingInfo()
    {
        Vector2 localPoint;
        if (uiCam == null) Debug.LogError("CONNECT \'UI CAMERA\' TO \'uiCam\' VARIABLE");
        RectTransformUtility.ScreenPointToLocalPointInRectangle(objToMove.parent as RectTransform, Input.mousePosition, uiCam, out localPoint);
        localPoint += objectOffset;
        objToMove.localPosition = localPoint;
    }

    public void FillDataUpgrade(ResourcesStruct res, int turnedOffForTurns)
    {
        _uiController.CursorFloatingWindowInfoDisplay(res,turnedOffForTurns,titleForUpgrade);
    }
    public void FillDataDemo(ResourcesStruct res)
    {
        _uiController.CursorFloatingWindowInfoDisplay(res, titleForSell);
    }
}
