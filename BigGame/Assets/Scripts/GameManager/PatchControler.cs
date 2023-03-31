using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;
using static CastleClass;
using static DrogaClass;

public class PatchControler : MonoBehaviour
{
    public static PatchControler Instance;

    [Header("Starting ")]
    public Castle PlayerCastle;
    public Castle ComputerCastle;

    public static List<Droga> PathWay = new List<Droga>();
    public List<Droga> pathwayDebug = new List<Droga>();

    public int pathLenght;

    private void Start()
    {
        Instance = this;
    }

    public void StartPath()
    {
        pathwayDebug = PathWay;
        pathLenght = PathWay.Count - 1;
    }


    #region Universal actions
    public void UnitAttack(UnitControler thisUnit, UnitControler targetUnit)
    {
        if (targetUnit != null)
        {
            thisUnit.SetTargetToAttack(targetUnit);
        }
    }

    public void UnitAttack(UnitControler thisUnit, CastleStats targetUnit)
    {
        if (targetUnit != null)
        {
            thisUnit.SetTargetToAttack(targetUnit);
        }
    }

    public void ClearWantingUnits(bool playerUnit)
    {
        foreach (var item in PathWay)
        {
            if (item.wantingUnit != null)
            {
                if (item.wantingUnit.GetComponent<UnitControler>().IsThisPlayerUnit() == playerUnit)
                {
                    item.wantingUnit = null;
                }
            }

        }
    }

    #endregion

    public void PlayerUnitPhase()
    {
        ClearWantingUnits(true);
        PlayerUnitPathAction();
        PlayerUnitCastleAction();        
    }

    #region Player Units Actions


    public void PlayerUnitPathAction()
    {
        for (int i = pathLenght; i >= 0; i--)
        {
            if (PathWay[i].unit == null)
            {
                continue;
            }
            if (!PathWay[i].unit.GetComponent<UnitControler>().IsThisPlayerUnit())
            {
                continue;
            }

            var thisUnit = PathWay[i].unit;
            var thisUnitController = thisUnit.GetComponent<UnitControler>();
            var thisUnitAttackReach = thisUnitController.ReturnAttackReach();
            var thisUnitMoveDistance = thisUnitController.ReturnMovmeDistance();

            bool target = false;

            for (int ii = 1; ii <= thisUnitAttackReach; ii++)
            {
                if (i + ii > pathLenght)
                {
                    UnitAttack(thisUnitController, ComputerCastle.castle.GetComponent<CastleStats>());
                    target= true;
                    break;
                }
                if (PathWay[i + ii].unit == null)
                {
                    continue;
                }
                if (PathWay[ii + i].unit.GetComponent<UnitControler>().IsThisPlayerUnit())
                {
                    continue;
                }
                UnitAttack(thisUnitController, PathWay[i + ii].unit.GetComponent<UnitControler>());
                target = true;
                break;
            }



            List<int> positions = new List<int>();
            PathWay[i].wantingUnit = thisUnit;
            for (int ii = 1; ii <= thisUnitMoveDistance; ii++)
            {
                if (i + ii > pathLenght)
                {
                    break;
                }

                if (PathWay[i + ii].wantingUnit != null)
                {
                    break;
                }

                if (PathWay[i + ii].wantingUnit == null)
                {
                    positions.Add(i + ii);
                    PathWay[i + ii].wantingUnit = thisUnit;
                    PathWay[i + ii - 1].wantingUnit = null;
                    continue;

                }




            }

            thisUnitController.SetWaypoints(positions);

            if (!target)
            {
                thisUnitController.MoveAction();
            }


        }


    }
    public void PlayerUnitCastleAction()
    {
        if (PlayerCastle.jednostka != null)
        {

            var thisUnit = PlayerCastle.jednostka;
            var thisUnitController = thisUnit.GetComponent<UnitControler>();
            var thisUnitAttackReach = thisUnitController.ReturnAttackReach();
            var thisUnitMoveDistance = thisUnitController.ReturnMovmeDistance();

            GameObject target = null;

            for (int i = 0; i < thisUnitAttackReach; i++)
            {
                if (i > pathLenght)
                {
                    break;
                }
                if (PathWay[i].unit == null)
                {
                    continue;
                }
                if (PathWay[i].unit.GetComponent<UnitControler>().IsThisPlayerUnit())
                {
                    continue;
                }
                target = PathWay[i].unit;
                break;

            }

            List<int> positions = new List<int>();
            for (int i = 0; i < thisUnitMoveDistance; i++)
            {

                if (PathWay[i].wantingUnit == null)
                {
                    positions.Add(i);
                    PathWay[i].wantingUnit = thisUnit;
                }
                else if (PathWay[i].wantingUnit.GetComponent<UnitControler>().IsThisPlayerUnit())
                {
                    break;
                }
                else if (!PathWay[i].wantingUnit.GetComponent<UnitControler>().IsThisPlayerUnit())
                {
                    if (PathWay[i].wantingUnit.GetComponent<UnitControler>().ReturnHp() - thisUnitController.ReturnDamage() <= 0)
                    {
                        positions.Add(i);
                        PathWay[i].wantingUnit = thisUnit;
                    }
                    break;
                }



            }


            UnitAttack(thisUnitController, target?.GetComponent<UnitControler>());

            thisUnitController.SetWaypoints(positions);

            if (target == null)
            {
                thisUnitController.MoveAction();
            }

            if ((positions?.Count ?? 0) > 0)
            {
                PlayerCastle.jednostka = null;
            }

        }
    }


    #endregion

    public void ComputerUnitPhaze()
    {


    }


}
