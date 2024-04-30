using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class InteractableHexRules
{
    [SerializeField] private bool apply;
    
    [Tooltip("Number in hexes (value of 2 is range of 2 hexes) - applied only if ")]
    [SerializeField] private int range;

    private float hexLength = 0.866025f; //fixed hex length

    public bool Apply
    {
        get => apply;
    }

    public int Range
    {
        get => range;
    }

    public float HexLength
    {
        get => hexLength;
    }
    
}
