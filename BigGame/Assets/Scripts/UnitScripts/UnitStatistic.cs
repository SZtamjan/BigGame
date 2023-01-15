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
    int movmentDistance = 1;
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

    public void DamageTaken(int obtained)
    {
        hp-= obtained;

        if (hp<=0)
        {
            Destroy(gameObject);
        }
    }




}