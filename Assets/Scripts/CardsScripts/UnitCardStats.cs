using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using NaughtyAttributes;
using static UnitsStatsClass;

public class UnitCardStats : MonoBehaviour
{
    [SerializeField] private CardScriptableObject stats; //Serialized only for debug purpose

    public CardScriptableObject Stats
    {
        get => stats;
        private set => stats = value;
    }

    [SerializeField] private new TextMeshProUGUI name;
    [SerializeField] private TextMeshProUGUI desc;
    [SerializeField] private TextMeshProUGUI cost;

    public void FillStats(CardScriptableObject newStats)
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

    [Button]//Guzik Do Testu
    public void XDDD()
    {
        var cd = Stats.GetStatsCard();
        foreach (var item in cd)
        {
            Debug.Log(item);
        }
    }
}