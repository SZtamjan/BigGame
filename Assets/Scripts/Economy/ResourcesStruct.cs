using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct ResourcesStruct
{
    private int gold;
    private int stone;
    private int wood;
    private int food;

    public int Gold
    {
        set
        {
            gold = value;
            UpdateUI();
        }
        get
        {
            return gold;
        }
    }
    public int Stone
    {
        set
        {
            stone = value;
            UpdateUI();
        }
        get
        {
            return stone;
        }
    }
    public int Wood
    {
        set
        {
            wood = value;
            UpdateUI();
        }
        get
        {
            return wood;
        }
    }
    public int Food
    {
        set
        {
            food = value;
            UpdateUI();
        }
        get
        {
            return food;
        }
    }

    private void UpdateUI()
    {
        UIController.Instance.EconomyUpdateResources(this);
    }
}
