using System;
using System.Collections;
using System.Collections.Generic;
using Economy.EconomyActions;
using Unity.VisualScripting;
using UnityEngine;

public class WorldUISwitcher : MonoBehaviour
{
    private Camera cam;
    private GameObject building;

    [SerializeField] private GameObject upgradeUI;

    private void Start()
    {
        cam = Camera.main;
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
                    upgradeUI.SetActive(false);
                    return;
                }

                SwitchActive();
            }
        }

        if (Input.GetMouseButtonDown(1))
        {
            upgradeUI.SetActive(false);
        }
    }

    private void SwitchActive()
    {
        EconomyOperations.Purchase(new ResourcesStruct(0, 0, 0, 0));
        if (upgradeUI.activeSelf)
        {
            upgradeUI.SetActive(false);
            return;
        }
        
        upgradeUI.SetActive(true);
    }
}
