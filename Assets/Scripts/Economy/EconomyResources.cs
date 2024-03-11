using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class EconomyResources : MonoBehaviour
{
    #region Resources

    private ResourcesStruct resources;

    public ResourcesStruct Resources
    {
        get => resources;
        private set => resources = value;
    }
    
    // public int cash;
    // public int gold;
    // public int stone;
    // public int wood;
    // public int food;

    #endregion
    
    [FormerlySerializedAs("playerCashSO")] public PlayerResources playerResourcesSo;

    public static EconomyResources Instance;
    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        FillResourcesOnStart();
    }

    private void FillResourcesOnStart()
    {
        resources.Gold = playerResourcesSo.gold;
        resources.Stone = playerResourcesSo.stone;
        resources.Wood = playerResourcesSo.wood;
        resources.Food = playerResourcesSo.food;
    }

    private void UIUpdate()
    {
        UIController.Instance.EconomyUpdateResources(resources);
    }

    public bool CanIBuy(int spend)
    {
        if (spend <= resources.Gold)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool Purchase(int spend)
    {
        bool tmp = CanIBuy(spend);
        if (tmp)
        {

            resources.Gold -= spend;
            return true;
        }
        else
        {
            return false;
        }

    }

    public void CashOnTurn()
    {
        resources.Gold += playerResourcesSo.cashCastleOnTurn;
    }

    public void AddCash(int value)
    {
        resources.Gold += value;
    }
    
}
