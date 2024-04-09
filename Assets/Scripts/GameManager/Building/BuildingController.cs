using System.Collections;
using System.Collections.Generic;
using Economy.EconomyActions;
using UnityEngine;
using UnityEngine.Serialization;

public class BuildingController : MonoBehaviour
{
    #region BuildingInfo

    private BuildingsScriptableObjects buildingInfo;

    public BuildingsScriptableObjects CurrentBuildingInfo
    {
        get => buildingInfo;
        private set => buildingInfo = value;
    }
    
    //Unit this building adds
    [SerializeField] private UnitScriptableObjects unitAdd; //Shown only for debug purpose
    
    //General info
    public WhichBudynek thisBudynekIs;
    private ResourcesStruct resourcesCurrentGain;
    private ResourcesStruct resourcesCurrentSell;
    private List<UpdateBuildingStruct> upgradeListThisBuilding;


    private GameObject terrainTypeThatWasThere;
    public GameObject ReturnTerrainTypeThatWasThere
    {
        get => terrainTypeThatWasThere;
        set => terrainTypeThatWasThere = value;
    }

    #endregion

    //Vars
    private UpdateBuildingStruct newLevelInfo;
    private int currentLevel = 0;
    private bool disabled = false;
    //private int disabledForTurns = 0; //byc moze nie bedzie potrzebne, mam w newLevelInfo te zmienna
    
    #region Events

    private void OnEnable()
    {
        EventManager.BuildingAction += BuildingActionOnTurn;
        EventManager.NewPlayerTurn += NewTurnTrigger;
    }

    private void OnDisable()
    {
        RemoveBuffs();
        EventManager.BuildingAction -= BuildingActionOnTurn;
        EventManager.NewPlayerTurn -= NewTurnTrigger;
    }

    #endregion

    private void ChangeState(BuildingStates newState)
    {
        switch (newState)
        {
            case BuildingStates.StartUpgrade:
                PrepareUpgrade();
                //DO NOT UPGRADE HERE ALREADY (or at least dont apply visual effects),
                //first need to wait for building to be turned back on after upgrade (after few turns)
                break;
            case BuildingStates.StartDisable:
                RemoveBuffs();
                CheckBuildingStatusAndApply();
                break;
            case BuildingStates.EndUpgrade:
                ApplyNewStats();
                break;
        }
    }
    
    public void FillNewStatsToThisBuilding(BuildingsScriptableObjects stats,int newLevel)
    {
        CurrentBuildingInfo = stats;
        
        thisBudynekIs = stats.whichBudynek;
        
        upgradeListThisBuilding = stats.buildingLevelsList;
        newLevelInfo = stats.buildingLevelsList[newLevel];
        
        UpdateModel(); //niepotrzebne bo bedzie prefab budynku i tak
        
        resourcesCurrentGain = stats.buildingLevelsList[newLevel].newResourcesGainOnTurn;
        unitAdd = stats.buildingLevelsList[newLevel].newUnit;
        
        Debug.LogWarning("Be careful with applyNewSell checkbox");
        if (stats.buildingLevelsList[newLevel].applyNewSell)
        {
            resourcesCurrentSell = stats.buildingLevelsList[newLevel].newSellValue;
        }
        
        if (unitAdd != null)
        {
            CardManager.instance.AddCardToDrawableCollection(unitAdd);
        }

        currentLevel = newLevel;
    }

    private void NewTurnTrigger()
    {
        ChangeState(BuildingStates.StartDisable);
    }

    private void PrepareUpgrade()
    {
        newLevelInfo.turnOffBuildingForTurns = upgradeListThisBuilding[currentLevel].turnOffBuildingForTurns;
        //New building model
        newLevelInfo.newMesh = upgradeListThisBuilding[currentLevel].newMesh;
        newLevelInfo.materials = upgradeListThisBuilding[currentLevel].materials;
        //New unit model
        newLevelInfo.newUnit = upgradeListThisBuilding[currentLevel].newUnit;
        //New gain value
        newLevelInfo.newResourcesGainOnTurn = upgradeListThisBuilding[currentLevel].newResourcesGainOnTurn;
        //New sell value
        newLevelInfo.applyNewSell = upgradeListThisBuilding[currentLevel].applyNewSell;
        newLevelInfo.newSellValue = upgradeListThisBuilding[currentLevel].newSellValue;
        
        
        ChangeState(BuildingStates.StartDisable);
    }

    private void RemoveBuffs()
    {
        if(unitAdd != null) CardManager.instance.RemoveCardToDrawableCollection(unitAdd);
    }
    
    private void CheckBuildingStatusAndApply()
    {
        if (newLevelInfo.turnOffBuildingForTurns > 0)
        {
            disabled = true;
            newLevelInfo.turnOffBuildingForTurns--;
        }
        else
        {
            disabled = false;
            ApplyNewStats();
        }
    }

    private void ApplyNewStats()
    {
        //Building
        UpdateModel(); //Mesh and materials
        
        //Unit
        if (newLevelInfo.newUnit != null)
        {
            unitAdd = newLevelInfo.newUnit;
            CardManager.instance.AddCardToDrawableCollection(unitAdd);
        }
        //Jeszcze trzeba dodac zeby w grze sie tez ulepszała a nie w samym budynku
        
        //Gain value
        resourcesCurrentGain = newLevelInfo.newResourcesGainOnTurn;
        
        //Sell value
        if (newLevelInfo.applyNewSell)
            resourcesCurrentSell = newLevelInfo.newSellValue;
    }

    private void UpdateModel()
    {
        Debug.Log("BUILDING MODEL CHANGE WIP");
        
        //Mesh
        GetComponent<MeshFilter>().mesh = newLevelInfo.newMesh;
        //Mesh Collider
        GetComponent<MeshCollider>().sharedMesh = newLevelInfo.newMesh;

        //Materials
        MeshRenderer mr = GetComponent<MeshRenderer>();
        mr.materials = newLevelInfo.materials;
    }
    
    private void SwitchCardToDrawAvailability(bool condition)
    {
        // True hides
        // False shows with current level
        if (condition)
        {
            
        }
        else
        {
            
        }
    }
    
    private void BuildingActionOnTurn()
    {
        if (!disabled)
        {
            EconomyOperations.AddResources(resourcesCurrentGain);
        }
    }

    #region DestroyBuilding

    public ResourcesStruct ReturnResourcesSellValue()
    {
        return resourcesCurrentSell;
    }

    #endregion
}

public enum BuildingStates
{
    SetUpUnit,
    StartUpgrade,
    StartDisable,
    EndUpgrade
}