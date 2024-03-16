using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class EconomyResources : MonoBehaviour
{
    #region Resources

    [SerializeField] private ResourcesStruct resources;

    public ResourcesStruct Resources
    {
        get => resources;
        set => resources = value;
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
        Instance = this;
    }
    private void Start()
    {
        FillResourcesOnStart();
    }

    private void FillResourcesOnStart()
    {
        //Change it to iteration through fields as in EconomyOperations
        Resources.Gold = playerResourcesSo.gold;
        Resources.Stone = playerResourcesSo.stone;
        Resources.Wood = playerResourcesSo.wood;
        Resources.Food = playerResourcesSo.food;
    }

    private void UIUpdate()
    {
        UIController.Instance.EconomyUpdateResources(Resources);
    }

    public bool CanIBuy(int spend)
    {
        if (spend <= Resources.Gold)
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

            Resources.Gold -= spend;
            return true;
        }
        else
        {
            return false;
        }

    }

    public void CashOnTurn()
    {
        Resources.Gold += playerResourcesSo.cashCastleOnTurn;
    }

    public void AddCash(int value)
    {
        Resources.Gold += value;
    }
    
}
