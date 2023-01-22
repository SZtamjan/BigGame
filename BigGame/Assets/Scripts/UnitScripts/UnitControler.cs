using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitControler : MonoBehaviour
{
    public UnitScriptableObjects unitScriptableObjects;
    public int hp;
    public int damage;
    public int movmentDistance;
    public int attackReach;
    public bool playersUnit;

    private void Start()
    {
        hp = unitScriptableObjects.hp;
        damage = unitScriptableObjects.damage;
        movmentDistance = unitScriptableObjects.movmentDistance;
        attackReach = unitScriptableObjects.attackReach;
        playersUnit = unitScriptableObjects.playersUnit;

    }
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
        hp -= obtained;

        if (hp <= 0)
        {
            Destroy(gameObject);
        }
    }




}