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
        if (EconomyOperations.CheckIfICanIAfford(GetComponent<UnitCardStats>().Stats.resources,false))
        {
            UnitSpawner.instance.SpawnMyUnit(gameObject, GetComponent<UnitCardStats>().Stats);
        }
    }
}
