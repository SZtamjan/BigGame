using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static SpellsStatsClass;


[CreateAssetMenu(fileName = "SpellCard", menuName = "Cards/SpellCard")]
public class SpellScriptableObjects : CardScriptableObject
{
    [Header("Statystyki ")]
    [SerializeField] int power;
    [SerializeField] bool playerSide = true;
    [SerializeField]SpellsScrptableObject spellsScrptableObject;
    public override void CardAction(GameObject karta, CardScriptableObject stats)
    {        
        SpellStats stat = (SpellStats)stats.GetStats();
        spellsScrptableObject.SpellAction(karta, stat.playersSide, stat.power);
    }

    public override object GetStats()
    {
        SpellStats stats = new SpellStats();
        stats.power=power;
        stats.playersSide=playerSide;
        return stats;

    }
}
