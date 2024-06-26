using System;
using System.Collections.Generic;
using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class TutorialSection
{
    [Header("Display Dialog Text")]
    public TextMeshProUGUI displayDialogOnObject;
    
    [Header("Display Dialog Art")]
    public Sprite zbikArt;
    public Image uiElementForZbik;
    
    [Header("Dialog")]
    public List<string> dialogFragment;
    public string additionalInfo;

    [Header("Bypass settings")] 
    public bool allowBuilding;
    public bool allowUnits;
    
    [Header("Interaction Background")]
    [Tooltip("Optional")] public GameObject uiBackground;
    [Tooltip("If uiBackground is not null then it decides what to wait for")] public bool waitForDialog;
}