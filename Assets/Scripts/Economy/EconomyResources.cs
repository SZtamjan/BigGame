using System;
using System.Collections;
using System.Collections.Generic;
using Economy.EconomyActions;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class EconomyResources : MonoBehaviour
{
    #region Resources

    [HideInInspector] [SerializeField] private ResourcesStruct _resources;

    public ResourcesStruct Resources
    {
        get => _resources;
        set => _resources = value;
    }
    
    // public int cash;
    // public int gold;
    // public int stone;
    // public int wood;
    // public int food;

    #endregion
    
    public PlayerResources playerResourcesSo;

    public static EconomyResources Instance;
    private void Awake()
    {
        if (_resources == null) _resources = new ResourcesStruct();
        Instance = this;
    }

    private void Start()
    {
        FillResourcesOnStart();
    }

    private void FillResourcesOnStart()
    {
        //Change it to iteration through fields as in EconomyOperations
        Resources.Gold = playerResourcesSo.playerResources.Gold - playerResourcesSo.resourcesOnTurn.Gold;
        Resources.Food = playerResourcesSo.playerResources.Food - playerResourcesSo.resourcesOnTurn.Food;
        Resources.Wood = playerResourcesSo.playerResources.Wood - playerResourcesSo.resourcesOnTurn.Wood;
        Resources.Stone = playerResourcesSo.playerResources.Stone - playerResourcesSo.resourcesOnTurn.Stone;
    }

    public void CashOnTurn()
    {
        EconomyOperations.AddResources(playerResourcesSo.resourcesOnTurn);
    }

    public void AddCash(int value)
    {
        Resources.Gold += value;
    }
    
}
