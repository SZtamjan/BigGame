using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitStatistic : MonoBehaviour
{
    [Header("Statystyki ")]
    [SerializeField]
    int unitHP = 100;
    [SerializeField]
    int damage = 10;
    [SerializeField]
    int movmentDistance = 1;
    [SerializeField]
    int attackReach = 1;
    [SerializeField]
    bool playersUnit = true;



    public int ReturnHp()
    {
        return unitHP;
    }

    public int ReturnDamage()
    {
        return damage;
    }

    public int ReturnMovmentDistance()
    {
        return movmentDistance;
    }
    public int ReturnattackReach()
    {
        return attackReach;
    }
    public bool IsThisPlayerUnit()
    {
        return playersUnit;
    }

}