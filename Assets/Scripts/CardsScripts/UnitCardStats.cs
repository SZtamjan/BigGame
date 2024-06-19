using System;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
using TMPro;
using UnityEngine.Serialization;
using UnityEngine.UI;
using NaughtyAttributes;
using static UnitsStatsClass;

public class UnitCardStats : MonoBehaviour
{
    private bool _disableFieldInInspector = true;
    [DisableIf("_disableFieldInInspector")] [SerializeField] private CardScriptableObject cardInfo; //Serialized only for debug purpose
    public CardScriptableObject CardInfo
    {
        get => cardInfo;
        private set => cardInfo = value;
    }

    [SerializeField] private new TextMeshProUGUI name;
    [SerializeField] private TextMeshProUGUI desc;
    [SerializeField] private TextMeshProUGUI gold;
    [SerializeField] private TextMeshProUGUI food;
    [SerializeField] private TextMeshProUGUI hp;
    [SerializeField] private TextMeshProUGUI damage;
    

    public void FillStats(CardScriptableObject newStats)
    {
        CardInfo = newStats;
        GetCardStats();
    }

    private void GetCardStats()
    {
        Sprite cardArtwork = CardInfo.artwork;

        gameObject.GetComponent<Image>().sprite = cardArtwork;
        
        name.text = CardInfo.name;
        desc.text = CardInfo.desc;
        
        gold.text = CardInfo.resources.Gold.ToString();
        food.text = CardInfo.resources.Food.ToString();
        
        var cd = CardInfo.GetStatsCard();
        if(cd == null) return;
        if(cd[1] < 0) return;
        hp.text = cd[0].ToString();
        damage.text = cd[1].ToString();
    }

    public TextMeshProUGUI[] ReturnTexts()
    {
        return new[] { name, desc, gold };
    }

    [Button]//Guzik Do Testu
    public void XDDD()
    {
        var cd = CardInfo.GetStatsCard();
        foreach (var item in cd)
        {
            Debug.Log(item);
        }
    }
}