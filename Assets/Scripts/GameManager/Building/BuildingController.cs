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
    
    //Upgrade
    private ResourcesStruct resourcesUpgradeCost;
    private ResourcesStruct resourcesCurrentMaxGain;
    private ResourcesStruct resourcesCurrentSell;
    private List<UpdateBuildingStruct> upgradeListThisBuilding;

    //previous terrain
    private GameObject terrainTypeThatWasThere;
    public GameObject ReturnTerrainTypeThatWasThere
    {
        get => terrainTypeThatWasThere;
        set => terrainTypeThatWasThere = value;
    }
    
    //interactable hexes
    private List<InteractableHex> _interactableHexes;
    private InteractableHexRules iRules;

    #endregion

    private BuildingStates currentState;
    
    //Vars
    private UpdateBuildingStruct nextLevelInfo;
    private int currentLevel = 0;
    private bool buildingMaxed = false;
    private int turnOffForTurns = 0;

    #region Properies

    public BuildingStates CurrentState
    {
        get => currentState;
    }

    public ResourcesStruct UpgradeCost
    {
        get => resourcesUpgradeCost;
    }
    
    public bool BuildingMaxed
    {
        get => buildingMaxed;
    }

    public int CurrentLevel
    {
        get => currentLevel;
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
                GetComponent<BuildingInfoSendToDisplayer>().CheckConditionsForButtonUpgradeVisibility();
                break;
        }
    }
    
    private void NewTurnTrigger() //Called at the start of every turn
    {
        StateManager(currentState);
    }
    
    private void TurnOffBuilding()
    {
        if (turnOffForTurns > 0)
        {
            turnOffForTurns--;
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
        thisBudynekIs = stats.whichBudynek;
        
        CurrentBuildingInfo = stats;

        upgradeListThisBuilding = stats.buildingLevelsList;
        
        iRules = stats.buildingLevelsList[newLevel].interactableHexRules;
        if(iRules.Apply) DetectInteractableHexes(iRules.HexLength * (float)iRules.Range); 
        
        //if next level exists
        if (stats.buildingLevelsList.Count > newLevel + 1)
        {
            resourcesUpgradeCost = stats.buildingLevelsList[newLevel+1].thisLevelCost;
            nextLevelInfo = stats.buildingLevelsList[newLevel+1];
            turnOffForTurns = stats.buildingLevelsList[newLevel+1].turnOffBuildingForTurns;
        }
        else
        {
            Debug.Log("LEVEL NA BUDYNKU OSIAGNIETO, TRZEBA WYLACZYC PRZYCISK");
            buildingMaxed = true;
        }
        
        currentLevel = newLevel;
        Debug.Log("building lvl " + currentLevel);
        
        UpdateModel();
        
        if (stats.buildingLevelsList[newLevel].newUnit != null)
        {
            unitAdd = stats.buildingLevelsList[newLevel].newUnit;
            CardManager.instance.AddCardToDrawableCollection(unitAdd);
        }
        
        resourcesCurrentMaxGain = stats.buildingLevelsList[newLevel].newResourcesGainOnTurn;

        Debug.LogWarning("Be careful with applyNewSell checkbox");
        if (stats.buildingLevelsList[newLevel].applyNewSell)
        {
            resourcesCurrentSell = stats.buildingLevelsList[newLevel].newSellValue;
        }
        
        currentState = BuildingStates.Normal;
    }
    
    private void DetectInteractableHexes(float range)
    {
        _interactableHexes = new List<InteractableHex>();

        Collider[] objectsInRange = Physics.OverlapSphere(transform.position, range);

        foreach (var objectInRange in objectsInRange)
        {
            objectInRange.TryGetComponent<InteractableHex>(out InteractableHex hex);
            if (hex == null) continue;
            if (hex.InteractWith == thisBudynekIs) _interactableHexes.Add(hex);
        }
    }

    private void RemoveBuffs()
    {
        if(unitAdd != null) CardManager.instance.RemoveCardToDrawableCollection(unitAdd);

        SaveAndChangeStateTo(BuildingStates.StartDisable);
    }

    private void UpdateModel()
    {
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
        if (iRules.Apply)
        { //if interactable applies
            CalculateResourcesFromInteractables();
        }
        else
        { //not interactable
            EconomyOperations.AddResources(resourcesCurrentMaxGain);
        }
        
    }

    private void CalculateResourcesFromInteractables()
    {
        foreach (var iHex in _interactableHexes)
        {
            //WIP
            Debug.Log("Calculowanie z interactable in progress");
        }
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