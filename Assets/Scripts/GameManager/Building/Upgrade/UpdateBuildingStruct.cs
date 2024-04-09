using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[System.Serializable]
public class UpdateBuildingStruct
{
    [Tooltip("Turns off building for X amount of turns")] public int turnOffBuildingForTurns;
    
    [Header("Building Model")]
    public Mesh newMesh;
    public Material[] materials;
    
    [Header("Unit SO")]
    public UnitScriptableObjects newUnit;

    [Header("Gain Value")]
    public ResourcesStruct newResourcesGainOnTurn;

    [Header("Sell value")]
    public bool applyNewSell;
    public ResourcesStruct newSellValue;
}
