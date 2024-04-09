using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static SpawnUnitsScriptableObject;
using static UnityEngine.Rendering.DebugUI;
[SelectionBase]
public class UnitControler : MonoBehaviour
{
    public UnitScriptableObjects unitScriptableObjects;
    public GameObject hpbar;
    private Gate _MyGate;

    [Header("Statystyki tylko do odczytu")]
    [SerializeField] private int hp;
    [SerializeField] private int hidenHP;

    [SerializeField] private int shield = 0;
    [SerializeField] private int hidenShield = 0;


    [SerializeField] private int damage;
    [SerializeField] private int attackReach;

    [SerializeField] private int movmentDistance;
    [SerializeField] private bool playersUnit;
    [SerializeField] private float speed;

    [SerializeField] private bool isAttacking = false;
    [SerializeField] private bool isMovving = false;
    [SerializeField] private bool isPojectileFlying = false;

    [SerializeField] private int IdleAnimationsNumber = 1;
    private bool _iMDying = false;
    
    [ShowIf("_RangedUnit")]
    [Header("Dla jednostek dystansowych")]
    public ProjectileController projectileToSpawn;
    [ShowIf("_RangedUnit")] public Transform projectileStartingPoint;
    private ProjectileController projectile;

    [SerializeField] private bool _SupportUnit = false;
    [ShowIf("_SupportUnit")][SerializeField] private int shieldPower;


    [SerializeField] private bool _mountedUnit = false;
    [ShowIf("_mountedUnit")]
    [SerializeField] private Animator _SecondAnimator;



    //-------------------------------------------------------
    private UnitControler targetUnitToAttack;
    private CastleStats targetCastleToAttack;
    private Gate targetGateToAttack;

    private List<int> wayPoints;
    private GameObject EnemyCastle;

    private Animator animator;


    void Start()
    {
        wayPoints = new List<int>();
        //SetStats();
        animator = GetComponent<Animator>();
        if (_mountedUnit && _SecondAnimator == null)
        {
            Debug.Log("Co� nie tak z mountem");
        }
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


    }

    public void setMyGate(Gate myGate)
    {
        _MyGate = myGate;
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

    public int ReturnShield()
    {
        return shield;
    }

    public int ReturnHiddenShield()
    {
        return hidenShield;
    }

    public int ReturnDamage()
    {
        return damage;
    }

    public int ReturnShieldPower()
    {
        return shieldPower;
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

    public bool IsSupportUnit()
    {
        return _SupportUnit;
    }

    public bool AmIDoingSomething()
    {
        if (isAttacking || isMovving || isPojectileFlying || _iMDying)
        {
            return true;
        }
        return false;
    }

    #endregion
    public void SpellDamageTaken(int obtained)
    {
        DamageTaken(obtained);
        HiddenDamageTaken(obtained);
    }
    public void DamageTaken(int obtained)
    {
        if (shield >= obtained)
        {
            shield -= obtained;

        }
        else
        {
            shield = 0;
            hp -= (obtained - shield);
        }

        if (hp <= 0)
        {
            animator.SetBool("death", true);
            _iMDying = true;
            if (_mountedUnit)
            {
                _SecondAnimator.SetBool("death", true);
            }
        }
        PlayHurt();
        hpbar.GetComponent<HpUnitsShow>().HPUpdate(hp);
        hpbar.GetComponent<HpUnitsShow>().ShieldUpdate(shield);


    }

    public void HiddenDamageTaken(int obtained)
    {
        if (hidenShield >= obtained)
        {
            hidenShield -= obtained;
        }
        else
        {
            hidenHP -= (obtained - hidenShield);
            if (hidenHP <= 0)
            {
                Destroy(gameObject, 8f);
            }
        }
    }

    public void SpellShieldTaken(int obtained)
    {
        shield += obtained;
        hpbar.GetComponent<HpUnitsShow>().ShieldUpdate(obtained);
        hidenShield += obtained;
    }

    public void ShieldTaken(int obtained)
    {
        shield = obtained;
        hpbar.GetComponent<HpUnitsShow>().ShieldUpdate(obtained);

    }
    public void HiddenShieldTaken(int obtained)
    {
        hidenShield = obtained;

    }

    public void DestroyMe()
    {
        Destroy(gameObject);
    }



    #region Play Animation

    public void PlayWalk()
    {
        if (!_iMDying)
        {
            animator.SetTrigger("walk");
            if (_mountedUnit)
            {
                _SecondAnimator.SetTrigger("walk");
            }
        }

    }
    public void PlayAttack()
    {
        if (!_iMDying)
        {
            animator.SetTrigger("attack");
            if (_mountedUnit)
            {
                _SecondAnimator.SetTrigger("attack");
            }

        }
    }

    public void PlaySupport()
    {
        if (!_iMDying)
        {
            animator.SetTrigger("support");
            if (_mountedUnit)
            {
                _SecondAnimator.SetTrigger("support");
            }
        }

    }
    public void PlayHurt()
    {

        animator.SetTrigger("hurt");
        if (_mountedUnit)
        {
            _SecondAnimator.SetTrigger("hurt");
        }
    }
    public void PlayIdle()
    {
        if (!_iMDying && !isAttacking && !isMovving)
        {
            animator.SetTrigger("idle");
            if (_mountedUnit)
            {
                _SecondAnimator.SetTrigger("idle");
            }
        }

    }

    public void PlayRandomIdle()
    {
        if (IdleAnimationsNumber < 1)
        {
            Debug.Log("co� posz�o nie tak z ilo�ci� animacji");
        }
        else
        {
            int randomAnimation = Random.Range(0, IdleAnimationsNumber) + 1;


            animator.SetInteger("idle_n", randomAnimation);

        }

        PlayIdle();
    }

    #endregion


    #region Attack method
    public void SetTargetToAttack(UnitControler targetUnit)
    {
        isAttacking = true;
        targetUnitToAttack = targetUnit;
        PlayAttack();

    }

    public void SetTargetToSupport(UnitControler targetUnit)
    {
        isAttacking = true;
        targetUnitToAttack = targetUnit;
        PlaySupport();

    }

    public void SetTargetToAttack(CastleStats targetUnit)
    {
        isAttacking = true;
        targetCastleToAttack = targetUnit;
        PlayAttack();
    }
    public void SetTargetToAttack(Gate targetUnit)
    {
        isAttacking = true;
        targetGateToAttack = targetUnit;
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
        else if (targetGateToAttack != null)
        {
            targetGateToAttack.DamageTaken(damage);
        }
        else
        {
            Debug.Log("Nie ma nic do ataku");
        }

        targetUnitToAttack = null;
        targetCastleToAttack = null;

    }

    public void SupportTarget()
    {
        if (targetUnitToAttack != null)
        {
            targetUnitToAttack.ShieldTaken(shieldPower);
        }

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
        if (targetGateToAttack != null)
        {
            return targetGateToAttack.transform.position;
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


        bool reserveNextHex = true;

        bool disableTransparentGate = true;

        while ((wayPoints?.Count ?? 0) > 0)
        {

            if (reserveNextHex)
            {
                if (wayPoints.First() > 0 && playersUnit)
                {
                    if (_MyGate.path[wayPoints.First()].unitWanting == null)
                    {
                        _MyGate.path[wayPoints.First()].unitWanting = this;

                        _MyGate.path[wayPoints.First() - direction].unitWanting = null;
                        reserveNextHex = false;

                    }

                }
                else if (wayPoints.First() < _MyGate.path.Count() - 1 && !playersUnit)
                {
                    if (_MyGate.path[wayPoints.First()].unitWanting == null)
                    {
                        _MyGate.path[wayPoints.First()].unitWanting = this;

                        _MyGate.path[wayPoints.First() - direction].unitWanting = null;
                        reserveNextHex = false;
                    }
                }
                else
                {
                    if (_MyGate.path[wayPoints.First()].unitWanting == null)
                    {
                        _MyGate.path[wayPoints.First()].unitWanting = this;
                        reserveNextHex = false;
                    }

                }
            }

            if (isAttacking)
            {
                yield return new WaitForEndOfFrame();
                continue;
            }


            if (!isAttacking)
            {
                if (_MyGate.path[wayPoints.First()].unitWanting == null || _MyGate.path[wayPoints.First()].unitWanting == this)
                {
                    if (disableTransparentGate && (wayPoints.First() == 1 || wayPoints.First() == _MyGate.path.Count - 2))
                    {
                        disableTransparentGate = false;
                        _MyGate.SetTransparent(1f);
                    }

                    if ((wayPoints?.Count ?? 0) > 0 && !wasThereWalk)
                    {
                        PlayWalk();
                        wasThereWalk = true;
                    }
                    transform.position = Vector3.MoveTowards(transform.position, _MyGate.path[wayPoints.First()].position, Time.deltaTime * speed);

                    if (Vector3.Distance(transform.position, _MyGate.path[wayPoints.First()].position) < 0.2f)
                    {
                        Vector3 lookAt;
                        if (playersUnit)
                        {
                            if (!(wayPoints[0] + direction > _MyGate.path.Count() - 1))
                            {
                                lookAt = _MyGate.path[wayPoints.First() + direction].position;
                            }
                            else
                            {
                                lookAt = EnemyCastle.transform.position;
                            }
                        }
                        else
                        {
                            if ((wayPoints[0] + direction > -1))
                            {
                                lookAt = _MyGate.path[wayPoints.First() + direction].position;
                            }
                            else
                            {
                                lookAt = EnemyCastle.transform.position;
                            }
                        }

                        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(lookAt - transform.position), 8f * Time.deltaTime);
                    }


                    if (Vector3.Distance(transform.position, _MyGate.path[wayPoints.First()].position) < 0.02f)
                    {

                        reserveNextHex = true;
                        wayPoints.RemoveAt(0);

                    }
                }

            }
            yield return null;

        }

        isMovving = false;
        if (wasThereWalk)
        {
            PlayIdle();
        }
        yield return null;
    }

    #endregion

}