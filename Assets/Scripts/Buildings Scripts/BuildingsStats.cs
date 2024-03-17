using System.Collections;
using System.Collections.Generic;
using Economy.EconomyActions;
using UnityEngine;
using UnityEngine.Serialization;

public class BuildingsStats : MonoBehaviour
{
    [SerializeField] private ResourcesStruct resourcesGain; //To be implemented
    
    public WhichBudynek thisBudynekIs;
    [SerializeField] private UnitScriptableObjects unitAdd;
    public GameObject terrainTypeThatWasThere;

    private BuildingsScriptableObjects thisBuildingStats;

    private void OnEnable()
    {
        EventManager.BuildingAction += BuildingActionOnTurn;
    }

    private void OnDisable()
    {
        if(unitAdd != null) CardManager.instance.RemoveCardToDrawableCollection(unitAdd);
        EventManager.BuildingAction -= BuildingActionOnTurn;
    }


    public void putStats(BuildingsScriptableObjects stats)
    {
        thisBuildingStats = stats;
        resourcesGain = stats.resourcesGainOnTurn;
        thisBudynekIs = stats.whichBudynek;
        unitAdd = stats.UnitAdd;
        if (unitAdd != null)
        {
            CardManager.instance.AddCardToDrawableCollection(unitAdd);
        }
    }

    public ResourcesStruct ReturnResourcesGain()
    {
        // tu w ramach potrzeb można dopisać jakieś warunki na generowanie pieniażków
        return resourcesGain;
    }

    public ResourcesStruct ReturnResources()
    {
        return thisBuildingStats.resourcesCost;
    }

    public ResourcesStruct ReturnResourcesSellValue()
    {
        return thisBuildingStats.resourcesSell;
    }
    void BuildingActionOnTurn()
    {
        EconomyOperations.AddResources(resourcesGain);
    }




}