using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Economy.EconomyActions;
using UnityEngine;

public class InteractableHex : MonoBehaviour
{
    [SerializeField] private WhichBudynek interactWith;
    [SerializeField] private ResourcesStruct hexResources;

    private Coroutine _repalaceHexCor;
    
    public WhichBudynek InteractWith
    {
        get => interactWith;
    }

    public ResourcesStruct HexResources
    {
        get
        {
            CheckIfEmptyAndSend();
            if(hexResources == null) Debug.LogError("xd");
            return hexResources;//ciekawe czy po return zadziala
        }
    }

    private void CheckIfEmptyAndSend()
    {
        PropertyInfo[] fields = typeof(ResourcesStruct).GetProperties(BindingFlags.Instance |
                                                                      BindingFlags.NonPublic |
                                                                      BindingFlags.Public);
        string dwa = gameObject.name.ToString();
        int fieldsCount = fields.Length;
        int counter = 0;
        foreach (var field in fields)
        {
            if ((int)field.GetValue(hexResources) == 0) counter++;
        }
        
        if (counter == fieldsCount && _repalaceHexCor == null) _repalaceHexCor = StartCoroutine(GetComponent<ReplaceHex>().ReplaceMe());
    }
}
