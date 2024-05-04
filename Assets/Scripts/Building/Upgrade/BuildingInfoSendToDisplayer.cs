using System;
using System.Collections;
using System.Collections.Generic;
using Economy.EconomyActions;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class BuildingInfoSendToDisplayer : MonoBehaviour
{
    private Camera cam;
    private GameObject building;

    private GameObject uiInfoDisplayGO;
    private DisplayBuildingInfo displayInfo;

    private void Start()
    {
        cam = Camera.main;
        uiInfoDisplayGO = DisplayBuildingInfo.Instance.gameObject;
        displayInfo = uiInfoDisplayGO.GetComponent<DisplayBuildingInfo>();
        //building = gameObject.transform.GetComponentInParent(WhichBudynek);
    }

    private void OnDisable()
    {
        if(uiInfoDisplayGO != null) uiInfoDisplayGO.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (EventSystem.current.IsPointerOverGameObject()) return;
            
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 100))
            {
                if (!ReferenceEquals(hit.collider.gameObject,gameObject))
                {
                    TurnOffWindow();
                    return;
                }

                SwitchActive();
            }
        }

        if (Input.GetMouseButtonDown(1))
        {
            TurnOffWindow();
        }
    }

    private void SwitchActive()
    {
        if (uiInfoDisplayGO.activeSelf)
        {
            TurnOffWindow();
            return;
        }

        TurnOnWindow();
    }

    public void TurnOffWindow()
    {
        uiInfoDisplayGO.SetActive(false);
    }

    private void TurnOnWindow()
    {
        StartCoroutine(WaitForBuildingSetUpAndLoadDisplayInfo());
    }

    private IEnumerator WaitForBuildingSetUpAndLoadDisplayInfo() //Necessary
    {
        //Fill info
        yield return new WaitUntil(() => TryGetComponent<BuildingController>(out var buildingController));
        yield return new WaitUntil(() => GetComponent<BuildingController>().CurrentBuildingInfo != null);
        displayInfo.FillDataToDisplayOnRightPanel(GetComponent<BuildingController>().CurrentBuildingInfo,gameObject);
        
        //Disable upgrade Button
        CheckConditionsForButtonUpgradeVisibility();
        
        uiInfoDisplayGO.SetActive(true);
        yield return null;
    }

    public void CheckConditionsForButtonUpgradeVisibility()
    {
        BuildingController myBController = GetComponent<BuildingController>();
        ShowUpgradeButton(true);
        
        if (!EconomyOperations.CheckIfICanIAfford(myBController.UpgradeCost))
        {
            ShowUpgradeButton(false);
            return;
        }

        if (myBController.CurrentState != BuildingStates.Normal)
        {
            ShowUpgradeButton(false);
            return;
        }

        if (myBController.BuildingMaxed)
        {
            ShowUpgradeButton(false);
            return;
        }
    }
    
    private void ShowUpgradeButton(bool value)
    {
        displayInfo.UpgradeButton.interactable = value;
    }
}
