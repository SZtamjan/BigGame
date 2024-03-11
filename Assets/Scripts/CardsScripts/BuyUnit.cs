using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Economy.EconomyActions;

public class BuyUnit : EconomyOperations
{
    public ResourcesStruct res;
    
    public void InitBuy()
    {
        if (Purchase(GetComponent<UnitCardStats>().Stats.resources))
        {
            SpawnerScript.instance.SpawnMyUnit(gameObject, GetComponent<UnitCardStats>().Stats);
        }
    }
}
