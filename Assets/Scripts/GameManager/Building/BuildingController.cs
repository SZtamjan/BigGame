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

    private BuildingStates currentState;
    
    //Vars
    private UpdateBuildingStruct nextLevelInfo;
    private int currentLevel = 0;
    private bool buildingMaxed = false;

    #region Properies

    public BuildingStates CurrentState
    {
        get => currentState;
    }
    
    public bool BuildingMaxed
    {
        get => buildingMaxed;
    }

    #endregion
    
    #region Events

    private void OnEnable()
    {
        EventManager.NewPlayerTurn += NewTurnTrigger;
    }

    private void OnDisable()
    {
        RemoveBuffs();
        EventManager.NewPlayerTurn -= NewTurnTrigger;
    }

    #endregion

    private void StateManager(BuildingStates newState)
    {
        Debug.Log("building in state: " + currentState);
        switch (newState)
        {
            case BuildingStates.Normal:
                BuildingActionsOnTurn();
                break;
            case BuildingStates.StartUpgrade:
                RemoveBuffs();
                break;
            case BuildingStates.StartDisable:
                TurnOffBuilding();
                break;
            case BuildingStates.EndUpgrade:
                FillNewStatsToThisBuilding(CurrentBuildingInfo,currentLevel+1);
                break;
        }
    }
    
    private void NewTurnTrigger() //Called at the start of every turn
    {
        StateManager(currentState);
    }
    
    private void TurnOffBuilding()
    {
        if (nextLevelInfo.turnOffBuildingForTurns > 0)
        {
            nextLevelInfo.turnOffBuildingForTurns--;
        }
        else
        {
            SaveAndChangeStateTo(BuildingStates.EndUpgrade);
        }
        // newLevelInfo.turnOffBuildingForTurns = upgradeListThisBuilding[currentLevel].turnOffBuildingForTurns;
        // //New building model
        // newLevelInfo.newMesh = upgradeListThisBuilding[currentLevel].newMesh;
        // newLevelInfo.materials = upgradeListThisBuilding[currentLevel].materials;
        // //New unit model
        // newLevelInfo.newUnit = upgradeListThisBuilding[currentLevel].newUnit;
        // //New gain value
        // newLevelInfo.newResourcesGainOnTurn = upgradeListThisBuilding[currentLevel].newResourcesGainOnTurn;
        // //New sell value
        // newLevelInfo.applyNewSell = upgradeListThisBuilding[currentLevel].applyNewSell;
        // newLevelInfo.newSellValue = upgradeListThisBuilding[currentLevel].newSellValue;

        //StateManager(BuildingStates.StartDisable);
    }

    
    
    public void FillNewStatsToThisBuilding(BuildingsScriptableObjects stats,int newLevel)
    {
        CurrentBuildingInfo = stats;
        
        thisBudynekIs = stats.whichBudynek;
        
        upgradeListThisBuilding = stats.buildingLevelsList;

        if (stats.buildingLevelsList.Count > newLevel + 1)
        {
            nextLevelInfo = stats.buildingLevelsList[newLevel+1];
        }
        else
        {
            Debug.Log("LEVEL NA BUDYNKU OSIAGNIETO, TRZEBA WYLACZYC PRZYCISK");
            buildingMaxed = true;
        }
        
        currentLevel = newLevel;
        Debug.Log("building lvl " + currentLevel);
        
        UpdateModel(); //niepotrzebne bo bedzie prefab budynku i tak
        
        if (stats.buildingLevelsList[newLevel].newUnit != null)
        {
            unitAdd = stats.buildingLevelsList[newLevel].newUnit;
            CardManager.instance.AddCardToDrawableCollection(unitAdd);
        }
        
        resourcesCurrentGain = stats.buildingLevelsList[newLevel].newResourcesGainOnTurn;

        Debug.LogWarning("Be careful with applyNewSell checkbox");
        if (stats.buildingLevelsList[newLevel].applyNewSell)
        {
            resourcesCurrentSell = stats.buildingLevelsList[newLevel].newSellValue;
        }

        currentState = BuildingStates.Normal;
    }

    private void RemoveBuffs()
    {
        if(unitAdd != null) CardManager.instance.RemoveCardToDrawableCollection(unitAdd);

        SaveAndChangeStateTo(BuildingStates.StartDisable);
    }

    private void UpdateModel()
    {
        Debug.Log("BUILDING MODEL CHANGE WIP");
        
        //Mesh
        GetComponent<MeshFilter>().mesh = upgradeListThisBuilding[currentLevel].newMesh;
        //Mesh Collider
        GetComponent<MeshCollider>().sharedMesh = upgradeListThisBuilding[currentLevel].newMesh;

        //Materials
        MeshRenderer mr = GetComponent<MeshRenderer>();
        mr.materials = upgradeListThisBuilding[currentLevel].materials;
    }

    private void BuildingActionsOnTurn()
    {
        EconomyOperations.AddResources(resourcesCurrentGain);
    }
    
    public void SaveAndChangeStateTo(BuildingStates newState)
    {
        currentState = newState;
        StateManager(newState);
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
    Normal,
    StartUpgrade,
    StartDisable,
    EndUpgrade
}