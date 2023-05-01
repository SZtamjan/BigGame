using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Economy : MonoBehaviour
{
    public TextMeshProUGUI moneyUI;
    public PlayerCash playerCashSO;
    private int cash;

    public static Economy Instance;
    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        cash = playerCashSO.playerCash;
        
        UIUpdate();
    }

    private void UIUpdate()
    {
        moneyUI.text = cash.ToString();
    }

    public bool CanIBuy(int spend)
    {
        if (spend <= cash)
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

            cash -= spend;
            UIUpdate();
            return true;
        }
        else
        {
            return false;
        }

    }

    public void CashOnTurn()
    {
        cash += playerCashSO.cashCastleOnTurn;
        UIUpdate();
    }

    
}
