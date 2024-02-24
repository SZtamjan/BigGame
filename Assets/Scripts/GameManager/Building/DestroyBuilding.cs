using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DestroyBuilding : MonoBehaviour
{
    public static DestroyBuilding Instance;
    
    private Camera cam;
    public LayerMask buildingMask;
    private Coroutine waitForInputCor;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        cam = Camera.main;
    }

    public void StartDestroying()
    {
        if (GameManager.instance.CanPlayerMove())
        {
            if (waitForInputCor == null)
            {
                waitForInputCor = StartCoroutine(StartWaitForPlayerInput());
            }
            else
            {
                Debug.Log("To nie powinno sie nigdy stac, juz jestem w trakcie demosowania budynku");
            }
            CloseMenu();
        }
        else
        {
            EconomyConditions.Instance.NotUrTurn();
            CloseMenu();
        }
    }

    private void CloseMenu()
    {
        UIController.Instance.BuildingCardsChangeShow(false);
    }

    private IEnumerator StartWaitForPlayerInput()
    {
        yield return new WaitForEndOfFrame();
        while (waitForInputCor != null)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray1 = cam.ScreenPointToRay(Input.mousePosition);
                RaycastHit raycastHit;
                
                if (!EventSystem.current.IsPointerOverGameObject())
                {
                    if (Physics.Raycast(ray1, out raycastHit, 100, buildingMask))
                    {
                        //Save clicked Building
                        GameObject hitBuilding = raycastHit.collider.gameObject;
                        
                        //Place previous ground
                        hitBuilding.GetComponent<BuildingsStats>().terrainTypeThatWasThere.SetActive(true);
                        
                        //Remove buffs and destroy
                        Building.Instance.RemoveBuilding(hitBuilding);
                    }
                }
                waitForInputCor = null;
            }
            yield return null;
        }
        
        yield return null;
    }

}
