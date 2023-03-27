using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Calculations : MonoBehaviour
{
    public GameObject gameManager;
    public GameObject economyObject;
    public TextMeshProUGUI moneyText;

    public int balansGracza = 10;
    
    public static Calculations ekonomia;

    private void Start()
    {
        UpdateMoney();
        ekonomia = this;
    }

    public void ZakupBudynku()
    {
        bool checker = true;
        

        //Prices prices = economyObject.GetComponent<Prices>();
        Debug.Log("Balans przed zakupem budynku: " + balansGracza);
        int budynek = Prices.prices.budynek;

        checker = CanIBuy(budynek);
        if (checker)
        {
            Buy(budynek);
        }
        checker = true;
    }

    public void Wiesniak()
    {
        bool checker = true;
        

        //Prices prices = economyObject.GetComponent<Prices>();
        Debug.Log("Balans przed podatkiem: " + balansGracza);
        int podatek = Prices.prices.podatek;

        checker = CanIBuy(podatek);
        if (checker)
        {
            Buy(podatek);
        }

        checker = true;
    }

    public bool CanIBuy(int value) //Jak bêdzie mo¿na w jednej turze kupowaæ kilka rzeczy to foreachem mo¿na
    {
        int result=0;
        result = balansGracza - value;
        if (result < 0)
        {
            Debug.Log("I can not buy");
            return false;
        }
        else
        {
            Debug.Log("I can buy");
            return true;
        }
    }

    public void Buy(int value)
    {

        balansGracza -= value;
        Debug.Log("Balans po podatku: " + balansGracza);
        

        UpdateMoney();
        
    }

    //Wyœwietlanie
    public void UpdateMoney()
    {
        moneyText.gameObject.GetComponent<TextMeshProUGUI>().text = balansGracza.ToString();
    }
}
