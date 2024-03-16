using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class BuildingCardGenTwo : MonoBehaviour
{
    [Header("Obiekt z budynkiem")]
    [SerializeField] private BuildingsScriptableObjects infoSource;
    
    [Header("Konfiguracja")]
    [SerializeField] private TextMeshProUGUI _buttonText;
    [SerializeField] private TextMeshProUGUI _buttonDescription;
    

    [SerializeField] private GameEvent buildingSelectedEvent;
    [SerializeField] private Color colorWhenSelected;
    private Color _originalColor;

    [Header("Shown only for debug purpose")]
    [SerializeField] private GameObject _stucture;
    [SerializeField] private ResourcesStruct _resources;
    [SerializeField] private ResourcesStruct _ResourcesGain;
    [SerializeField] private string _name;
    [SerializeField] private string _description;
    
    //scripts
    private BuildMenuManager _buildMenuManager;

    private void Start()
    {
        _originalColor = GetComponent<Image>().color;
        _buildMenuManager = BuildMenuManager.Instance;
        
        _stucture = infoSource.Budynek;
        _resources = infoSource.resourcesCost;
        _name = infoSource.name;
        _ResourcesGain = infoSource.resourcesGainOnTurn;
        _description = infoSource.desc;
        _buttonText.text = $"Buduj {_name}";
        _buttonDescription.text = $"kosztuje {_resources.Gold}, a daje {_ResourcesGain},\n{_description}";

        _buttonDescription.gameObject.SetActive(false);
    }

    public void SendInfoToRightPanel()
    {
        buildingSelectedEvent.Raise(); //Co to robi? xd
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