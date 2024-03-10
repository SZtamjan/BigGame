using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuyUnit : MonoBehaviour
{
    public void InitBuy()
    {
        SpawnerScript.instance.SpawnMyUnit(gameObject, GetComponent<UnitCardStats>().Stats);
    }
}
