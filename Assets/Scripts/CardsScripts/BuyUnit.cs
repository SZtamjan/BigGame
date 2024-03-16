using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using Economy.EconomyActions;

public class BuyUnit : EconomyOperations
{
    public void InitBuy()
    {
        if (CheckIfICanIAfford(GetComponent<UnitCardStats>().Stats.resources,false))
        {
            Debug.Log("worked");
            SpawnerScript.instance.SpawnMyUnit(gameObject, GetComponent<UnitCardStats>().Stats);
        }
    }
}
