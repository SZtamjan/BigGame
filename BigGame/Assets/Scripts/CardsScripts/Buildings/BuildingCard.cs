using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.UI;

public class BuildingCard : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI _butonText;
    [SerializeField] private TextMeshProUGUI _toolTipTex;
    [SerializeField] private BuildingsScriptableObjects infoSource;
    [SerializeField] private GameObject _stucture;
    [SerializeField] private int _cost;
    [SerializeField] private string _name;
    [SerializeField] private string _description;
    [SerializeField] private int _moneyGain;


    public void InitBuyBuilding()
    {
        bool CanIBuy = Economy.Instance.CanIBuy(infoSource.cost);
        if(CanIBuy)
        {
            StartBulding();
        }
        else
        {
            EconomyConditions.Instance.NotEnoughCash();
        }
    }
    
    public void StartBulding()
    {
        UIController.Instance.BuildingCardsChangeShow(false);
        Building.Instance.StartBuilding(infoSource);
    }

    

    void OnMouseEnter()
    {

        _toolTipTex.gameObject.SetActive(true);
    }

    void OnMouseExit()
    {
        _toolTipTex.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        _toolTipTex.gameObject.SetActive(false);
    }
    private void Start()
    {
        _stucture = infoSource.Budynek;
        _cost = infoSource.cost;
        _name = infoSource.name;
        _moneyGain = infoSource.moneyGain;
        _description = infoSource.desc;
        _butonText.text = $"Buduj {_name}";
        _toolTipTex.text = $"kosztuje {_cost}, a daje {_moneyGain},\n{_description}";

        _toolTipTex.gameObject.SetActive(false);
    }


}
