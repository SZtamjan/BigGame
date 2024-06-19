using System;
using UnityEngine;
using UnityEngine.Serialization;
using static UnitsStatsClass;

[CreateAssetMenu(fileName = "UnitCard", menuName = "Cards/UnitStats")]
public class UnitScriptableObjects : CardScriptableObject
{
    //Prefabs
    [Header("Jednostka ")]
    public GameObject unit;

    //Staty Jednostki
    [Header("Statystyki ")]
    [SerializeField]
    public int hp = 5;

    [SerializeField]
    public int damage = 2;

    [SerializeField]
    public int movmentDistance = 1;

    [SerializeField]
    public int attackReach = 1;

    [SerializeField]
    public float moveSpeed = 0.5f;

    public override void CardAction(GameObject karta, CardScriptableObject stats)
    {
        UnitSpawner.instance.SpawnMyUnit(karta, stats);
    }

    public override int[] GetStatsCard()
    {
        return new int[] { hp, damage };
    }

    public override object GetStats()
    {
        UnitsStats stats = new UnitsStats();
        stats.unit = unit;
        stats.hp = hp;
        stats.damage = damage;
        stats.movmentDistance = movmentDistance;
        stats.attackReach = attackReach;
        stats.playersSide = playersSide;
        stats.moveSpeed = moveSpeed;

        return stats;
    }
}