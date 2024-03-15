using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using Economy.EconomyActions;

public class BuyUnit : EconomyOperations
{
    public void InitBuy()
    {
        //Zakomentowane jest git, po testach tylko to zostawic
        // if (CheckIfICanIAfford(GetComponent<UnitCardStats>().Stats.resources))
        // {
        //     Debug.Log("worked");
        //     SpawnerScript.instance.SpawnMyUnit(gameObject, GetComponent<UnitCardStats>().Stats);
        // }
        
        if (CheckIfICanIAfford(GetComponent<UnitCardStats>().Stats.resources))
        {
            Debug.Log("worked");
            SpawnerScript.instance.SpawnMyUnit(gameObject, GetComponent<UnitCardStats>().Stats);
        }
    }
}
