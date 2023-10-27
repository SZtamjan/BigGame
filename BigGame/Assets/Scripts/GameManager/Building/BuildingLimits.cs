using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Limit", menuName = "ScriptableObjects/Buildings/BuildingLimits")]
public class BuildingLimits : ScriptableObject
{
    public int maxIlosc = 0;
    public WhichBudynek jakiBudynek;
}