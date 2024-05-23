using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class ResourcesStruct
{
    [HideInInspector] public UnityEvent updatedResources;
    
    [SerializeField] private int gold;
    [SerializeField] private int food;
    [SerializeField] private int wood;
    [SerializeField] private int stone;

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
        if(updatedResources != null) updatedResources.Invoke();
    }
    
    
    //Constructors
    public ResourcesStruct(int gold,int food, int wood, int stone)
    {
        Gold = gold;
        Food = food;
        Wood = wood;
        Stone = stone;
    }

    public ResourcesStruct()
    {
        
    }
}
