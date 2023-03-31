using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using static DrogaClass;

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
    [SerializeField] private bool isAttacking = false;

    private UnitControler targetUnitToAttack;
    private CastleStats targetCastleToAttack;

    private List<int> wayPoints;
    private GameObject EnemyCastle;

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

        if (playersUnit)
        {
            EnemyCastle = GameManager.gameManager.GetComponent<PatchControler>().ComputerCastle.castle;
        }
        else
        {
            EnemyCastle = GameManager.gameManager.GetComponent<PatchControler>().PlayerCastle.castle;
        }
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

    public int ReturnMovmeDistance()
    {
        return movmentDistance;
    }

    public int ReturnAttackReach()
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
        PlayHurt();
        hpbar.GetComponent<HpUnitsShow>().HPUpdate(hp);
    }



    #region Play Animation

    public void PlayWalk()
    {
        animator.SetTrigger("walk");
    }
    public void PlayAttack()
    {
        animator.SetTrigger("attack");
    }
    public void PlayHurt()
    {
        animator.SetTrigger("hurt");
    }
    public void PlayIdle()
    {
        animator.SetTrigger("idle");
    }

    #endregion

    public void SetTargetToAttack(UnitControler targetUnit)
    {
        targetUnitToAttack = targetUnit;
        isAttacking = true;
        PlayAttack();

    }
    public void SetTargetToAttack(CastleStats targetUnit)
    {
        targetCastleToAttack = targetUnit;
        isAttacking = true;
        PlayAttack();

    }

    public void AttackTarget()
    {
        if (targetUnitToAttack != null)
        {
            targetUnitToAttack.DamageTaken(damage);
        }
        else if (targetCastleToAttack!=null)
        {
            targetCastleToAttack.DamageTaken(damage);
        }
        else
        {
            Debug.Log("Nie ma nic do ataku");
        }

        targetUnitToAttack = null;
        targetCastleToAttack = null;

    }

    public void SetWaypoints(List<int> pathNumber)
    {
        wayPoints = pathNumber;
    }

    public void MoveAction()
    {
        StartCoroutine(MovingAction());
    }

    private IEnumerator MovingAction()
    {
        bool wasThereWalk = false;
        if ((wayPoints?.Count ?? 0) > 0)
        {
            PlayWalk();
            wasThereWalk = true;
        }

        bool dupa = true;

        while ((wayPoints?.Count ?? 0) > 0)
        {
            if (dupa && wayPoints.First() > 0)
            {
                if (PatchControler.PathWay[wayPoints.First()].unit == null)
                {
                    PatchControler.PathWay[wayPoints.First()].unit = gameObject;
                    if (wayPoints.First() > 0)
                    {
                        PatchControler.PathWay[wayPoints.First() - 1].unit = null;
                        dupa = false;
                    }
                }

            }
            else
            {
                PatchControler.PathWay[wayPoints.First()].unit = gameObject;
            }

            if (!isAttacking)
            {
                if (PatchControler.PathWay[wayPoints.First()].unit == gameObject)
                {

                    transform.position = Vector3.MoveTowards(transform.position, PatchControler.PathWay[wayPoints.First()].coordinations, Time.deltaTime * 0.5f);

                    if (Vector3.Distance(transform.position, PatchControler.PathWay[wayPoints.First()].coordinations) < 0.2f)
                    {
                        Vector3 lookAt;

                        if (!(wayPoints[0] + 1 > PatchControler.PathWay.Count() - 1))
                        {
                            lookAt = PatchControler.PathWay[wayPoints.First() + 1].coordinations;
                        }
                        else
                        {
                            lookAt = EnemyCastle.transform.position;
                        }

                        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(lookAt - transform.position), 8f * Time.deltaTime);
                    }


                    if (Vector3.Distance(transform.position, PatchControler.PathWay[wayPoints.First()].coordinations) < 0.02f)
                    {

                        dupa = true;
                        wayPoints.RemoveAt(0);

                    }
                }

            }
            yield return null;

        }


        if (wasThereWalk)
        {
            PlayIdle();
        }
        yield return null;
    }



}