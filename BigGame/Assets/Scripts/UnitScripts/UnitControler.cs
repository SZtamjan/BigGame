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
    [SerializeField] private int hidenHP;
    [SerializeField] private int damage;
    [SerializeField] private int movmentDistance;
    [SerializeField] private int attackReach;
    [SerializeField] private bool playersUnit;
    [SerializeField] private float speed;

    [SerializeField] private bool isAttacking = false;
    [SerializeField] private bool isMovving = false;
    [SerializeField] private bool isPojectileFlying = false;

    [Header("Dla jednostek dystansowych")]
    public ProjectileController projectileToSpawn;
    public Transform projectileStartingPoint;

    private ProjectileController projectile;

    //-------------------------------------------------------
    private UnitControler targetUnitToAttack;
    private CastleStats targetCastleToAttack;

    private List<int> wayPoints;
    private GameObject EnemyCastle;

    private Animator animator;
    void Start()
    {
        wayPoints = new List<int>();
        SetStats();
    }
    public void SetSO(UnitScriptableObjects stats)
    {
        unitScriptableObjects = stats;
        SetStats();
    }

    private void SetStats()
    {
        hp = unitScriptableObjects.hp;
        hidenHP = hp;
        damage = unitScriptableObjects.damage;
        movmentDistance = unitScriptableObjects.movmentDistance;
        attackReach = unitScriptableObjects.attackReach;
        playersUnit = unitScriptableObjects.playersUnit;
        speed = unitScriptableObjects.moveSpeed;

        hpbar.GetComponent<HpUnitsShow>().MaxHP(hp);
        animator = GetComponent<Animator>();

        if (playersUnit)
        {
            EnemyCastle = GameManager.gameManager.GetComponent<PathControler>().ComputerCastle.castle;
        }
        else
        {
            EnemyCastle = GameManager.gameManager.GetComponent<PathControler>().PlayerCastle.castle;
        }
    }

    #region return stats
    public int ReturnHp()
    {
        return hp;
    }
    public int ReturnHiddenHp()
    {
        return hidenHP;
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

    public bool AmIDoingSomething()
    {
        if (isAttacking || isMovving || isPojectileFlying)
        {
            return true;
        }
        return false;
    }

    #endregion

    public void DamageTaken(int obtained)
    {
        hp -= obtained;

        if (hp <= 0)
        {
            animator.SetBool("death", true);
        }
        PlayHurt();
        hpbar.GetComponent<HpUnitsShow>().HPUpdate(hp);


    }

    public void HiddenDamageTaken(int obtained)
    {
        hidenHP -= obtained;
        if (hidenHP <= 0)
        {
            Destroy(gameObject, 8f);
        }
    }

    public void DestroyMe()
    {
        Destroy(gameObject);
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


    #region Attack method
    public void SetTargetToAttack(UnitControler targetUnit)
    {
        isAttacking = true;
        targetUnitToAttack = targetUnit;
        PlayAttack();

    }
    public void SetTargetToAttack(CastleStats targetUnit)
    {
        isAttacking = true;
        targetCastleToAttack = targetUnit;
        PlayAttack();
    }

    public void AttackTarget()
    {
        if (targetUnitToAttack != null)
        {
            targetUnitToAttack.DamageTaken(damage);
        }
        else if (targetCastleToAttack != null)
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


    public void RangeAttack()
    {
        isPojectileFlying = true;

        projectile = Instantiate(projectileToSpawn, projectileStartingPoint.position, projectileStartingPoint.rotation, gameObject.transform.parent);
        Vector3 target = GetTargetToShoot();
        projectile.SetTaget(target);
        projectile.StartFlying();
        StartCoroutine(IsProjectileFlying());
    }

    private Vector3 GetTargetToShoot()
    {
        if (targetUnitToAttack != null)
        {
            Vector3 target = targetUnitToAttack.transform.position;
            target.y += 0.319f;
            return target;
        }
        if (targetCastleToAttack != null)
        {
            return targetCastleToAttack.transform.position;
        }
        return transform.position;
    }

    private IEnumerator IsProjectileFlying()
    {
        while (projectile != null)
        {
            yield return null;
        }

        isPojectileFlying = false;
        AttackTarget();
        yield return null;
    }

    #endregion


    #region Move method
    public void SetWaypoints(List<int> pathNumber)
    {
        wayPoints.AddRange(pathNumber);
    }


    public void MoveAction()
    {
        if (!isMovving)
        {
            isMovving = true;
            StartCoroutine(MovingAction());
        }

    }

    private IEnumerator MovingAction()
    {
        int direction;
        if (playersUnit)
        {
            direction = 1;
        }
        else
        {
            direction = -1;
        }

        bool wasThereWalk = false;


        bool dupa = true;

        while ((wayPoints?.Count ?? 0) > 0)
        {

            if (dupa && wayPoints.First() > 0 && playersUnit)
            {
                if (PathControler.PathWay[wayPoints.First()].wantingUnit == null)
                {
                    PathControler.PathWay[wayPoints.First()].wantingUnit = gameObject;

                    PathControler.PathWay[wayPoints.First() - direction].wantingUnit = null;
                    dupa = false;

                }

            }
            else if (dupa && wayPoints.First() < PathControler.PathWay.Count() - 1 && !playersUnit)
            {
                if (PathControler.PathWay[wayPoints.First()].wantingUnit == null)
                {
                    PathControler.PathWay[wayPoints.First()].wantingUnit = gameObject;

                    PathControler.PathWay[wayPoints.First() - direction].wantingUnit = null;
                    dupa = false;
                }
            }
            else
            {
                if (PathControler.PathWay[wayPoints.First()].wantingUnit == null)
                {
                    PathControler.PathWay[wayPoints.First()].wantingUnit = gameObject;
                }

            }
            
            if (isAttacking)
            {
                yield return new WaitForEndOfFrame();
                continue;
            }

            

            if (!isAttacking)
            {
                if (PathControler.PathWay[wayPoints.First()].wantingUnit == null || PathControler.PathWay[wayPoints.First()].wantingUnit == gameObject)
                {
                    if ((wayPoints?.Count ?? 0) > 0 && !wasThereWalk)
                    {
                        PlayWalk();
                        wasThereWalk = true;
                    }
                    transform.position = Vector3.MoveTowards(transform.position, PathControler.PathWay[wayPoints.First()].coordinations, Time.deltaTime * speed);

                    if (Vector3.Distance(transform.position, PathControler.PathWay[wayPoints.First()].coordinations) < 0.2f)
                    {
                        Vector3 lookAt;
                        if (playersUnit)
                        {
                            if (!(wayPoints[0] + direction > PathControler.PathWay.Count() - 1))
                            {
                                lookAt = PathControler.PathWay[wayPoints.First() + direction].coordinations;
                            }
                            else
                            {
                                lookAt = EnemyCastle.transform.position;
                            }
                        }
                        else
                        {
                            if ((wayPoints[0] + direction > 0))
                            {
                                lookAt = PathControler.PathWay[wayPoints.First() + direction].coordinations;
                            }
                            else
                            {
                                lookAt = EnemyCastle.transform.position;
                            }
                        }

                        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(lookAt - transform.position), 8f * Time.deltaTime);
                    }


                    if (Vector3.Distance(transform.position, PathControler.PathWay[wayPoints.First()].coordinations) < 0.02f)
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
        isMovving = false;
        yield return null;
    }

    #endregion

}