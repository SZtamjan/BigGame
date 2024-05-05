using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "ScriptableObjects", menuName = "Spells/SpellChangeCardToUnit")]
public class SpellChangeCardToUnit : SpellsScrptableObject
{

    private GameObject _Karta;
    public override void SpellAction(GameObject karta, bool playerUnit = true, int power = 1)
    {

        karta.GetComponent<Image>().color = CardManager.instance.selectedCardColor;
        _Karta = karta;
        UIController.Instance.EnableCardButton(false);
        SpellManager.Instance.SelectCard(DoSomethingWithThisCard);
    }


    public void DoSomethingWithThisCard(GameObject card)
    {
        _Karta.GetComponent<Image>().color = CardManager.instance.defaultCardColor;
        if (card==null)
        {
            Debug.Log("null");
        }
        else if (card==_Karta)
        {
            Debug.Log("Ta sama karta");
        }
        else
        {
            var _kartaStat = _Karta.GetComponent<UnitCardStats>();
            var newStats = card.GetComponent<UnitCardStats>().Stats;
            _kartaStat.FillStats(newStats);

            Debug.Log(card.name);

        }
        UIController.Instance.EnableCardButton(true);

    }

}
