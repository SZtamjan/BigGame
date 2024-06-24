using System.Collections;
using System.Collections.Generic;
using System.Reflection;
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

    public int TurnOffForTurns
    {
        get => turnOffForTurns;
    }

    public BuildingStates CurrentState
    {
        get => currentState;
    }

    public ResourcesStruct UpgradeCost => resourcesUpgradeCost;

    public ResourcesStruct ResourcesCurrentMaxGain => resourcesCurrentMaxGain;

    public ResourcesStruct ResourcesCurrentSell => resourcesCurrentSell;
    
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
      // zakomentowane bo robi problemy w sumie nie wiemy po co to jest xd
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
                FixTurnOffFor();
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

    private void FixTurnOffFor()
    {
        //it fixes a misscalculation - without it, game upgrades a building one tour before it should do so
        turnOffForTurns++;
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
            ResourcesStruct substractedValue = new ResourcesStruct();
            
            PropertyInfo[] fields = typeof(ResourcesStruct).GetProperties(BindingFlags.Instance |
                                                                          BindingFlags.NonPublic |
                                                                          BindingFlags.Public);
            foreach (var field in fields)
            {
                int fieldValueBeforeCalc = (int)field.GetValue(iHex.HexResources);
                int fieldMaxGain = (int)field.GetValue(resourcesCurrentMaxGain);
                field.SetValue(iHex.HexResources,CalculateUnit(fieldMaxGain,fieldValueBeforeCalc));

                int substractedFieldValue = fieldValueBeforeCalc - (int)field.GetValue(iHex.HexResources);
                field.SetValue(substractedValue,substractedFieldValue);
            }

            EconomyOperations.AddResources(substractedValue);
        }
    }

    private int CalculateUnit(int maxGain, int hexUnit)
    {
        for (int i = 0; i < maxGain; i++)
        {
            if(hexUnit <= 0)
            {
                //hex jest juz pusty i mozna cos tutaj wykonac z tej okazji
                //natomiast uruchomi sie jezeli tylko jeden surowiec dojdzie do zera
                //jak np jest 20 drewna i 10 jedzenia i gracz bieze 1 co ture z obu, to akcja bedzie wykonana gdy jedzenie dojdzie do zera
                break;
            }

            hexUnit--;
        }

        return hexUnit;
    }
    
    public void SaveAndChangeStateTo(BuildingStates newState)
    {
        currentState = newState;
        StateManager(newState);
    }
}

public enum BuildingStates
{
    Normal,
    StartUpgrade,
    StartDisable,
    EndUpgrade
}