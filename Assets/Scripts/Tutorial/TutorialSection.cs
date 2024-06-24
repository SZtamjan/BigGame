using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[Serializable]
public class TutorialSection
{
    public TextMeshProUGUI displayDialogOnObject;
    [Header("")] public List<string> dialogFragment;
    public GameObject uiBackground;
}