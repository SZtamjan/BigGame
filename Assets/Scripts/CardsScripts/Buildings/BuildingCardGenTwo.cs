using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class BuildingCardGenTwo : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _buttonText;
    [SerializeField] private TextMeshProUGUI _buttonDescription;
    [SerializeField] private BuildingsScriptableObjects infoSource;

    [SerializeField] private GameEvent buildingSelectedEvent;
    [SerializeField] private Color colorWhenSelected;
    private Color _originalColor;

    [Header("Shown only for debug purpose")]
    [SerializeField] private GameObject _stucture;
    [SerializeField] private int _cost;
    [SerializeField] private string _name;
    [SerializeField] private string _description;
    [SerializeField] private int _moneyGain;
    
    
    
    //scripts
    private BuildMenuManager _buildMenuManager;

    private void Start()
    {
        _originalColor = GetComponent<Image>().color;
        _buildMenuManager = BuildMenuManager.Instance;
        
        _stucture = infoSource.Budynek;
        _cost = infoSource.cost;
        _name = infoSource.name;
        _moneyGain = infoSource.moneyGain;
        _description = infoSource.desc;
        _buttonText.text = $"Buduj {_name}";
        _buttonDescription.text = $"kosztuje {_cost}, a daje {_moneyGain},\n{_description}";

        _buttonDescription.gameObject.SetActive(false);
    }

    public void SendInfoToRightPanel()
    {
        buildingSelectedEvent.Raise();
        SelectCard();
        _buildMenuManager.FillData(infoSource);
    }

    private void SelectCard()
    {
        GetComponent<Image>().color = colorWhenSelected;
    }

    public void UnselectCard()
    {
        GetComponent<Image>().color = _originalColor;
    }
    
    
    
    
    
}