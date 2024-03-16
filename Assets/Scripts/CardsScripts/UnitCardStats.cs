using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UnitCardStats : MonoBehaviour
{
    [SerializeField] private UnitScriptableObjects stats; //Serialized only for debug purpose
    public UnitScriptableObjects Stats
    {
        get => stats;
        private set => stats = value;
    }
    
    [SerializeField] private new TextMeshProUGUI name;
    [SerializeField] private TextMeshProUGUI desc;
    [SerializeField] private TextMeshProUGUI cost;

    public void FillStats(UnitScriptableObjects newStats)
    {
        Stats = newStats;
        
        GetCardStats();
    }
    
    private void GetCardStats()
    {
        Sprite cardArtwork = Stats.artwork;

        gameObject.GetComponent<Image>().sprite = cardArtwork;
        name.text = Stats.name;
        desc.text = Stats.desc;
        cost.text = Stats.resources.Gold.ToString();
    }

    public TextMeshProUGUI[] ReturnTexts()
    {
        return new[] { name, desc, cost };
    }
}
