using System;
using System.Collections.Generic;
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
    
    [Header("Interaction Background")]
    [Tooltip("Optional")] public GameObject uiBackground;
}