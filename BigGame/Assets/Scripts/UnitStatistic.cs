using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitStatistic : MonoBehaviour
{
    [Header("Statystyki ")]
    [SerializeField]
    int hp = 100;
    [SerializeField]
    int damage = 10;
    [SerializeField]
    int movment = 1;
    [SerializeField]
    int attackReach = 1;
    [SerializeField]
    bool playersUnit = true;



    public int ReturnHp()
    {
        return hp;
    }

    public int ReturnDamage()
    {
        return damage;
    }

    public int ReturnMovment()
    {
        return movment;
    }
    public int ReturnattackReach()
    {
        return attackReach;
    }
    public bool ReturnattackplayersUnit()
    {
        return playersUnit;
    }





}