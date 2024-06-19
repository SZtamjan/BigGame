using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "ScriptableObjects", menuName = "Spells/SpellChangeCardToUnit")]
public class SpellChangeCardToUnit : SpellsScrptableObject
{
    [SerializeField]
    private List<CardScriptableObject> ICanChangeInTo;
    private GameObject _ThisCard;
    public override void SpellAction(GameObject karta, bool playerUnit = true, int power = 1)
    {

        karta.GetComponent<Image>().color = CardManager.instance.selectedCardColor;
        _ThisCard = karta;
        UIController.Instance.EnableCardButton(false);
        SpellManager.Instance.SelectCard(DoSomethingWithThisCard);
    }


    public void DoSomethingWithThisCard(GameObject SelectedCard)
    {
        _ThisCard.GetComponent<Image>().color = CardManager.instance.defaultCardColor;
        if (SelectedCard==null)
        {
            Debug.Log("null");
        }
        else if (SelectedCard==_ThisCard)
        {
            Debug.Log("Ta sama karta");
        }

        else if (ICanChangeInTo.Count==0)
        {
            var _ThisCardStat = _ThisCard.GetComponent<UnitCardStats>();
            var newStats = SelectedCard.GetComponent<UnitCardStats>().CardInfo;
            _ThisCardStat.FillStats(newStats);

            Debug.Log($"Zamieniam się w {newStats.name}");

        }
        else if (checkIfCanChange(SelectedCard))
        {
            var _ThisCardStat = _ThisCard.GetComponent<UnitCardStats>();
            var newStats = SelectedCard.GetComponent<UnitCardStats>().CardInfo;
            _ThisCardStat.FillStats(newStats);

            Debug.Log($"Zamieniam się w {newStats.name}");
        }
        else
        {
            
            var newStats = SelectedCard.GetComponent<UnitCardStats>().CardInfo;
            Debug.Log($"Nie zamieniam się w {newStats.name}");
        }


        UIController.Instance.EnableCardButton(true);

    }

    private bool checkIfCanChange(GameObject SelectedCard)
    {
        var SelectedCardStats = SelectedCard.GetComponent<UnitCardStats> ();
        foreach (var item in ICanChangeInTo)
        {
            if (item.name == SelectedCardStats.CardInfo.name)
            {
                return true;
            }
        }
        return false;
    }

}
