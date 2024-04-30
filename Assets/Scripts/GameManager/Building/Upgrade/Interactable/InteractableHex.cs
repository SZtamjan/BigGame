using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableHex : MonoBehaviour
{
    [SerializeField] private WhichBudynek interactWith;
    [SerializeField] private ResourcesStruct hexResources;

    public WhichBudynek InteractWith
    {
        get => interactWith;
    }

    public ResourcesStruct HexResources
    {
        get => hexResources;
    }
}
