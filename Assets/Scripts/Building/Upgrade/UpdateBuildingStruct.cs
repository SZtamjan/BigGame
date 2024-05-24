using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[System.Serializable]
public class UpdateBuildingStruct
{
    [Tooltip("Turns off building for X amount of turns")] public int turnOffBuildingForTurns;
    
    [Header("Interactable Hex Config")]
    public InteractableHexRules interactableHexRules;
    
    [Header("Building Model")]
    public Mesh newMesh;
    public Material[] materials;
    
    [Header("Unit SO")]
    public UnitScriptableObjects newUnit;

    [Header("Cost of this level")]
    public ResourcesStruct thisLevelCost;
    
    [Header("Gain Value")]
    public ResourcesStruct newResourcesGainOnTurn;

    [Header("Sell value")]
    public bool applyNewSell=true;
    public ResourcesStruct newSellValue;
}
