using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitControler : MonoBehaviour
{
    public UnitScriptableObjects unitScriptableObjects;
    public GameObject hpbar;
   
    [Header("Statystyki tylko do odczytu")]
    [SerializeField] private int hp;
    [SerializeField] private int damage;
    [SerializeField] private int movmentDistance;
    [SerializeField] private int attackReach;
    [SerializeField] private bool playersUnit;

    private Animator animator;
    private void Start()
    {
        hp = unitScriptableObjects.hp;
        damage = unitScriptableObjects.damage;
        movmentDistance = unitScriptableObjects.movmentDistance;
        attackReach = unitScriptableObjects.attackReach;
        playersUnit = unitScriptableObjects.playersUnit;

        hpbar.GetComponent<HpUnitsShow>().MaxHP(hp);
        animator = GetComponent<Animator>();

    }
    #region return stats
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

    #endregion

    public void DamageTaken(int obtained)
    {
        hp -= obtained;

        if (hp <= 0)
        {
            animator.SetBool("death", true);
            Destroy(gameObject, 8f);
        }
        Invoke("PlayHurt", 0.8f);
        hpbar.GetComponent<HpUnitsShow>().HPUpdate(hp);
    }



    #region Play Animation

    public void PlayMove()
    {
        animator.SetTrigger("walk");
        StartCoroutine(GetComponent<UnitMove>().MoveThisUnit());
    }
    public void PlayAttack()
    {
        animator.SetTrigger("attack");
    }
    public void PlayHurt()
    {

        animator.SetTrigger("hurt");
    }




    #endregion

    public void MoveUnit(Vector3 pos, Vector3 rot)
    {
        gameObject.GetComponent<UnitMove>().AddToDestination(pos, rot);
    }
}