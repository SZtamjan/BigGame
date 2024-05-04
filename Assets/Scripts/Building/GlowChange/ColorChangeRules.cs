using NaughtyAttributes;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
[CreateAssetMenu(fileName = "ColorRules", menuName = "ScriptableObjects/Buildings/ColorRules")]
public class ColorChangeRules : ScriptableObject
{
    public List<WhatWhere> rules;

    public LayerMask BuidableLayer;
    public Color green;
    public Color grey;

    public bool CheckType(WhichBudynek? type, string tag)
    {
        if (type == null) return false;

        List<string> tagi = GetTagsForBuilding(type);
        bool toReturn = tagi.Contains(tag);

        return toReturn;
    }

    [System.Serializable]
    public class WhatWhere
    {
        public WhichBudynek budynek;
        [Tag] public List<string> Tags;

    }

    private List<string> GetTagsForBuilding(WhichBudynek? buildingType)
    {
        var matchingRule = rules.FirstOrDefault(rule => rule.budynek == buildingType);
        return matchingRule != null ? matchingRule.Tags : new List<string>();
    }

    public bool CheckLayer(LayerMask layer)
    {
        return (1 << layer) == BuidableLayer.value; ;
    }




}

