using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Money : MonoBehaviour
{
    public GameObject uiMoney;


    public int amount = 0;
    public void HereComesTheMoney() //https://www.youtube.com/watch?v=TeXatquVqAc
    {
        uiMoney.GetComponent<TextMeshProUGUI>().text = amount.ToString();
    }

    public bool CanIBuy(int checker)
    {
        if (amount >= checker)
        {

            return true;
        }
        else
        {

            return false;
        }
        
    }

    public void ModifyMoney(int wallet)
    {
        
        amount += wallet;
    }
}
