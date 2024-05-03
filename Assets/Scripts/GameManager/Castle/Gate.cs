using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;
using static PathClass;
using static UnityEditor.Progress;

[SelectionBase]
public class Gate : MonoBehaviour
{
    [SerializeField] private bool isPlayerSide = false;

    public List<Path> path = new List<Path>();
    [SerializeField][Tooltip("dobrze działa 0.8")] private float searchRadius = 0.8f;

    [Tag] public string newTag;
    [SerializeField] private Gate _secondGate;
    [SerializeField] private Castle _MyCastle;


    [SerializeField, Foldout("Przezroczystość")] private float fadeDuration = 0.66f;
    [SerializeField, Foldout("Przezroczystość"), Tooltip("index materiału przeznaczonego do przezroczystości")] private int materialIndex = 1;
    private Material _material;

    private Coroutine ditterowanie;




    public void GeneratePath()
    {
        newTag = CastlesController.Instance.ReturnNextFreeTag();
        gameObject.tag = newTag;
        path = new List<Path> { new Path { position = transform.position } };
        _secondGate = NewPath().GetComponent<Gate>();
        _secondGate.tag = newTag;
        path.Add(new Path { position = _secondGate.transform.position });

        _secondGate.SetSecoundGate(this);
        _secondGate.SetPath(path);

    }

    private void Start()
    {
        MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
        _material = meshRenderer.materials[materialIndex];

    }

    public void DamageTaken(int damege)
    {
        _MyCastle.HpChange -= damege;
    }

    #region Unit Attaack

    public void UnitAttack(UnitControler thisUnit, UnitControler targetUnit)
    {
        if (targetUnit != null)
        {
            thisUnit.SetTargetToAttack(targetUnit);
            targetUnit.HiddenDamageTaken(thisUnit.ReturnDamage());
        }
    }

    public void UnitAttack(UnitControler thisUnit, Gate targetUnit)
    {
        if (targetUnit != null)
        {
            thisUnit.SetTargetToAttack(targetUnit);
        }
    }

    public void UnitSupport(UnitControler thisUnit, UnitControler targetUnit)
    {
        if (targetUnit != null)
        {
            thisUnit.SetTargetToSupport(targetUnit);
            targetUnit.HiddenShieldTaken(thisUnit.ReturnShieldPower());
        }
    }

    void ClearWaintingPatch()
    {
        foreach (var pole in path)
        {
            pole.unitWanting = null;
        }
    }

    #endregion

    #region Player Units Actions

    [Button]
    public void PlayerUnitPhase()
    {
        ClearWaintingPatch();
        PlayerUnitPathAttack();
        PlayerUnitPathWalk();
    }

    private void PlayerUnitPathAttack()
    {
        int pathLenght = path.Count - 2;
        for (int i = pathLenght; i >= 0; i--)
        {
            if (path[i].unitMain == null)
            {
                continue;
            }
            if (!path[i].unitMain.GetComponent<UnitControler>().IsThisPlayerUnit())
            {
                continue;
            }
            var thisUnit = path[i].unitMain;
            var thisUnitController = thisUnit.GetComponent<UnitControler>();
            if (thisUnitController.ReturnHiddenHp() <= 0)
            {
                continue;
            }

            var thisUnitAttackReach = thisUnitController.ReturnAttackReach();
            var isThisUnitSupport = thisUnitController.IsSupportUnit();
            var thisUnitShieldPower = thisUnitController.ReturnShieldPower();




            for (int ii = 1; ii <= thisUnitAttackReach; ii++)
            {
                if (i + ii > pathLenght)
                {
                    UnitAttack(thisUnitController, _secondGate);

                    break;
                }
                if (path[i + ii].unitMain == null)
                {
                    continue;
                }
                if (path[ii + i].unitMain.GetComponent<UnitControler>().IsThisPlayerUnit())
                {
                    if (isThisUnitSupport && path[ii + i].unitMain.GetComponent<UnitControler>().ReturnHiddenShield() < thisUnitShieldPower)
                    {
                        UnitSupport(thisUnitController, path[ii + i].unitMain.GetComponent<UnitControler>());
                        break;
                    }
                    continue;
                }
                if (path[ii + i].unitMain.GetComponent<UnitControler>().ReturnHiddenHp() <= 0)
                {
                    continue;
                }
                UnitAttack(thisUnitController, path[i + ii].unitMain.GetComponent<UnitControler>());

                break;
            }
        }
    }


    private void PlayerUnitPathWalk()
    {
        int pathLenght = path.Count - 2;

        for (int i = pathLenght; i >= 0; i--)
        {
            if (path[i].unitMain == null)
            {
                continue;
            }
            if (!path[i].unitMain.GetComponent<UnitControler>().IsThisPlayerUnit())
            {
                continue;
            }

            var thisUnit = path[i].unitMain;
            var thisUnitController = thisUnit.GetComponent<UnitControler>();

            if (thisUnitController.ReturnHiddenHp() <= 0)
            {
                continue;
            }

            var thisUnitMoveDistance = thisUnitController.ReturnMovmeDistance();
            List<int> positions = new List<int>();
            path[i].unitWanting = thisUnit;



            for (int ii = 1; ii <= thisUnitMoveDistance; ii++)
            {
                if (i + ii > pathLenght)
                {
                    break;
                }
                if (path[i + ii].unitMain == null)
                {
                    positions.Add(i + ii);
                    path[i + ii].unitMain = thisUnit;
                    path[i + ii - 1].unitMain = null;                    
                    continue;

                }

                if (path[i + ii].unitMain != null)
                {
                    var nextUnitOnPathController = path[i + ii].unitMain.GetComponent<UnitControler>();
                    if (!nextUnitOnPathController.IsThisPlayerUnit() && (nextUnitOnPathController.ReturnHiddenHp() <= 0))
                    {
                        positions.Add(i + ii);
                        path[i + ii].unitMain = thisUnit;
                        path[i + ii - 1].unitMain = null;
                        continue;
                    }
                    break;
                }
            }

            thisUnitController.SetWaypoints(positions);

            if (!thisUnitController.AmIDoingSomething())
            {
                thisUnitController.MoveAction();

            }

        }



    }



    #endregion

    #region ENEMY Units Actions

    [Button]
    private void testEnemyMovment()
    {
        foreach (var item in CastlesController.Instance.enemyCastle.gates)
        {
            item.EnemyUnitPhase();
        }
    }
    public void EnemyUnitPhase()
    {
        ClearWaintingPatch();
        EnemyUnitPathAttack();
        EnemyUnitPathMove();
    }
    private void EnemyUnitPathAttack()
    {
        int pathLenght = path.Count - 1;
        for (int i = 1; i <= pathLenght; i++)
        {
            if (path[i].unitMain == null)
            {
                continue;
            }
            if (path[i].unitMain.GetComponent<UnitControler>().IsThisPlayerUnit())
            {
                continue;
            }
            var thisUnit = path[i].unitMain;
            var thisUnitController = thisUnit.GetComponent<UnitControler>();
            if (thisUnitController.ReturnHiddenHp() <= 0)
            {
                continue;
            }
            var thisUnitAttackReach = thisUnitController.ReturnAttackReach();

            for (int ii = 0; ii <= thisUnitAttackReach; ii++)
            {
                if (i - ii < 1)
                {
                    UnitAttack(thisUnitController, this);

                    break;
                }
                if (path[i - ii].unitMain == null)
                {
                    continue;
                }
                if (!path[i - ii].unitMain.GetComponent<UnitControler>().IsThisPlayerUnit())
                {
                    continue;
                }
                if (path[i - ii].unitMain.GetComponent<UnitControler>().ReturnHiddenHp() <= 0)
                {
                    continue;
                }
                UnitAttack(thisUnitController, path[i - ii].unitMain.GetComponent<UnitControler>());
                break;
            }
        }
    }

    private void EnemyUnitPathMove()
    {
        int pathLenght = path.Count - 1;
        for (int i = 1; i <= pathLenght; i++)
        {
            if (path[i].unitMain == null)
            {
                continue;
            }
            if (path[i].unitMain.GetComponent<UnitControler>().IsThisPlayerUnit())
            {
                continue;
            }

            var thisUnit = path[i].unitMain;
            var thisUnitController = thisUnit.GetComponent<UnitControler>();

            if (thisUnitController.ReturnHiddenHp() <= 0)
            {
                continue;
            }

            var thisUnitMoveDistance = thisUnitController.ReturnMovmeDistance();
            List<int> positions = new List<int>();
            path[i].unitWanting = thisUnit;

            for (int ii = 1; ii <= thisUnitMoveDistance; ii++)
            {
                if (i - ii < 1)
                {
                    break;
                }
                if (path[i - ii].unitMain == null)
                {
                    positions.Add(i - ii);
                    path[i - ii].unitMain = thisUnit;
                    path[i - ii + 1].unitMain = null;
                    continue;

                }

                if (path[i - ii].unitMain != null)
                {
                    var nextUnitOnPathController = path[i - ii].unitMain.GetComponent<UnitControler>();
                    if (nextUnitOnPathController.IsThisPlayerUnit() && (nextUnitOnPathController.ReturnHiddenHp() <= 0))
                    {
                        positions.Add(i - ii);
                        path[i - ii].unitMain = thisUnit;
                        path[i - ii + 1].unitMain = null;
                        continue;
                    }
                    break;
                }
            }

            thisUnitController.SetWaypoints(positions);

            if (!thisUnitController.AmIDoingSomething())
            {
                thisUnitController.MoveAction();

            }



        }
    }

    #endregion

    #region przezroczystości

    [Button]
    public void Test0()
    {
        SetTransparent(GameManager.Instance.GateTransparency);
    }
    [Button]

    public void Test1()
    {
        SetTransparent(1f);
    }

    public void SetTransparent(float transparency)
    {
        if (_material != null)
        {
            if (ditterowanie != null)
            {
                StopCoroutine(ditterowanie);
            }
            ditterowanie = StartCoroutine(ChangeDithering(transparency));
        }

    }

    private IEnumerator ChangeDithering(float target)
    {
        float time = 0f;
        float startDither = _material.GetFloat("_DitherThreshold");
        while (fadeDuration > time)
        {
            float ditter = Mathf.SmoothStep(startDither, target, time);
            time += Time.deltaTime / fadeDuration;
            _material.SetFloat("_DitherThreshold", ditter);
            yield return null;
        }
        _material.SetFloat("_DitherThreshold", target);

        yield return null;
    }





    #endregion



    #region path creation

    public void SetPath(List<Path> path)
    {
        this.path = path;
    }
    public void SetIfplayerOrNot(bool isPlayer)
    {
        isPlayerSide = isPlayer;
    }

    public void SetSecoundGate(Gate gate)
    {
        _secondGate = gate;

    }

    public void SetMyCastle(Castle castle)
    {
        _MyCastle = castle;
    }

    public GameObject NewPath()
    {
        GameObject toReturn = null;

        List<GameObject> ControlList = new List<GameObject>();

        List<GameObject> hits = GetHits(transform.position, "Path");
        hits = RemoveDuplicatesFromList(hits, ControlList);
        ControlList.AddRange(hits);
        int loops = 0;
        while (loops < 1000)
        {
            hits = GetHits(ControlList.Last().transform.position, "Path");
            if (hits.Count == 0)
            {
                break;
            }
            hits = RemoveDuplicatesFromList(hits, ControlList);
            ControlList.AddRange(hits);


            loops++;
        }


        foreach (var item in ControlList)
        {
            item.tag = newTag;
            Path hex = new Path { position = item.transform.position };
            hex.position.y += 0.13f;
            path.Add(hex);
        }

        toReturn = GetHits(ControlList.Last().transform.position, "Gate").Last();


        return toReturn;

    }

    List<GameObject> GetHits(Vector3 position, string tag)
    {
        List<GameObject> hits = new List<GameObject>();
        Collider[] colliders = Physics.OverlapSphere(position, searchRadius);
        foreach (Collider collider in colliders)
        {
            if (collider.CompareTag(tag))
            {
                hits.Add(collider.gameObject);
            }
        }

        return hits;
    }

    List<GameObject> RemoveDuplicatesFromList(List<GameObject> sourceList, List<GameObject> referenceList)
    {
        List<GameObject> duplicates = new List<GameObject>();

        foreach (GameObject item in sourceList)
        {
            if (referenceList.Contains(item))
            {
                duplicates.Add(item);
            }
        }

        foreach (GameObject duplicate in duplicates)
        {
            sourceList.Remove(duplicate);
        }

        return sourceList;
    }

    internal void ClearPathFromWanwingUnit()
    {
        foreach (var item in path)
        {
            item.unitWanting = null;
        };
    }

    #endregion


}
