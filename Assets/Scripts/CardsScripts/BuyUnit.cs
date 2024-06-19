using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using Economy.EconomyActions;

public class BuyUnit : MonoBehaviour
{
    public void InitBuy()
    {
        var unitCardStats = GetComponent<UnitCardStats>();
        if (EconomyOperations.CheckIfICanIAfford(unitCardStats.CardInfo.resources,false))
        {
            //UnitSpawner.instance.SpawnMyUnit(gameObject, GetComponent<UnitCardStats>().Stats);
            
            unitCardStats.CardInfo.CardAction(gameObject, unitCardStats.CardInfo);
        }
    }
}