using System.Collections;
using System.Collections.Generic;
using UnityEditor.TextCore.Text;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class ResourcesStruct
{
    public UnityEvent updatedResources;
    
    [SerializeField] private int gold;
    [SerializeField] private int stone;
    [SerializeField] private int wood;
    [SerializeField] private int food;

    public int Gold
    {
        get
        {
            return gold;
        }
        set
        {
            gold = value;
            PropertySetter();
        }
    }
    public int Stone
    {
        get
        {
            return stone;
        }
        set
        {
            stone = value;
            PropertySetter();
        }
    }
    public int Wood
    {
        get
        {
            return wood;
        }
        set
        {
            wood = value;
            PropertySetter();
        }
    }
    public int Food
    {
        get
        {
            return food;
        }
        set
        {
            food = value;
            PropertySetter();
        }
    }

    private void PropertySetter()
    {
        UpdateUI();
        InvokeUpdatedResources();
    }
    
    private void UpdateUI()
    {
        UIController.Instance.EconomyUpdateResources(this);
    }

    private void InvokeUpdatedResources()
    {
        updatedResources.Invoke();
    }
    
    
    //Constructors
    public ResourcesStruct( int gold, int stone, int wood, int food)
    {
        Gold = gold;
        Stone = stone;
        Wood = wood;
        Food = food;
    }
}
