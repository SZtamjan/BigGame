using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using Economy.EconomyActions;

public class BuyUnit : EconomyOperations
{
    private UnitCardStats unitCardStats;
    public void Start()
    {
        unitCardStats = GetComponent<UnitCardStats>();
    }
    public void InitBuy()
    {
        if (CheckIfICanIAfford(unitCardStats.Stats.resources,false))
        {
            //UnitSpawner.instance.SpawnMyUnit(gameObject, unitCardStats.Stats);
            unitCardStats.Stats.CardAction(gameObject, unitCardStats.Stats);
        }
    }
}
