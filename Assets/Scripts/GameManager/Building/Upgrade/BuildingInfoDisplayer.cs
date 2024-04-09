using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BuildingInfoDisplayer : MonoBehaviour
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

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
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
        yield return new WaitUntil(() => TryGetComponent<BuildingController>(out var buildingController));
        yield return new WaitUntil(() => GetComponent<BuildingController>().CurrentBuildingInfo != null);
        displayInfo.FillDataToDisplayOnRightPanel(GetComponent<BuildingController>().CurrentBuildingInfo,gameObject);
        uiInfoDisplayGO.SetActive(true);
        yield return null;
    }
}
