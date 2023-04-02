using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using UnityEngine;
using UnityEngine.UIElements;
using static CastleClass;
using static DrogaClass;
using static GameManager;

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
    public void StartNewPathWay()
    {
        PathWay = new List<Droga>();
    }
    public void StartPath()
    {
        pathwayDebug = PathWay;
        pathLenght = PathWay.Count - 1;
    }


    #region Universal Attack Actions
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

    #endregion

    public void PlayerUnitPhase()
    {
        PlayerUnitPathAction();
        PlayerUnitCastleAction();
    }

    #region Player Units Actions


    public void PlayerUnitPathAction()
    {
        //select units
        for (int i = pathLenght; i >= 0; i--)
        {
            if (PathWay[i].unitMain == null)
            {
                continue;
            }
            if (!PathWay[i].unitMain.GetComponent<UnitControler>().IsThisPlayerUnit())
            {
                continue;
            }

            var thisUnit = PathWay[i].unitMain;
            var thisUnitController = thisUnit.GetComponent<UnitControler>();
            var thisUnitAttackReach = thisUnitController.ReturnAttackReach();
            var thisUnitMoveDistance = thisUnitController.ReturnMovmeDistance();

            //Unit attack
            bool target = false;
            for (int ii = 1; ii <= thisUnitAttackReach; ii++)
            {
                if (i + ii > pathLenght)
                {
                    UnitAttack(thisUnitController, ComputerCastle.castle.GetComponent<CastleStats>());
                    target = true;
                    break;
                }
                if (PathWay[i + ii].unitMain == null)
                {
                    continue;
                }
                if (PathWay[ii + i].unitMain.GetComponent<UnitControler>().IsThisPlayerUnit())
                {
                    continue;
                }
                UnitAttack(thisUnitController, PathWay[i + ii].unitMain.GetComponent<UnitControler>());
                target = true;
                break;
            }


            //Unit Move
            List<int> positions = new List<int>();
            PathWay[i].unitMain = thisUnit;
            for (int ii = 1; ii <= thisUnitMoveDistance; ii++)
            {
                if (i + ii > pathLenght)
                {
                    break;
                }

                if (PathWay[i + ii].unitMain != null)
                {
                    var nextUnitOnPathController = PathWay[i + ii].unitMain.GetComponent<UnitControler>();
                    if (!(nextUnitOnPathController.IsThisPlayerUnit()) && (nextUnitOnPathController.ReturnHp() - thisUnitController.ReturnDamage() <= 0))
                    {
                        positions.Add(i + ii);
                        PathWay[i + ii].unitMain = thisUnit;
                        PathWay[i + ii - 1].unitMain = null;
                        continue;
                    }
                    break;
                }

                if (PathWay[i + ii].unitMain == null)
                {
                    positions.Add(i + ii);
                    PathWay[i + ii].unitMain = thisUnit;
                    PathWay[i + ii - 1].unitMain = null;
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

            //Unit Attack
            bool target = false;
            for (int i = 0; i < thisUnitAttackReach; i++)
            {
                if (i > pathLenght)
                {
                    UnitAttack(thisUnitController, ComputerCastle.castle.GetComponent<CastleStats>());
                    target = true;
                    break;
                }
                if (PathWay[i].unitMain == null)
                {
                    continue;
                }
                if (PathWay[i].unitMain.GetComponent<UnitControler>().IsThisPlayerUnit())
                {
                    continue;
                }
                UnitAttack(thisUnitController, PathWay[i].unitMain.GetComponent<UnitControler>());
                target = true;
                break;

            }

            //Unit Move
            List<int> positions = new List<int>();
            for (int i = 0; i < thisUnitMoveDistance; i++)
            {
                if (i > pathLenght)
                {
                    break;
                }
                if (PathWay[i].unitMain == null)
                {
                    positions.Add(i);
                    PathWay[i].unitMain = thisUnit;
                    if (i > 0)
                    {
                        PathWay[i - 1].unitMain = null;
                    }
                    continue;
                }
                if (PathWay[i].unitMain.GetComponent<UnitControler>().IsThisPlayerUnit())
                {
                    break;
                }
                if (!PathWay[i].unitMain.GetComponent<UnitControler>().IsThisPlayerUnit())
                {
                    if (PathWay[i].unitMain.GetComponent<UnitControler>().ReturnHp() - thisUnitController.ReturnDamage() <= 0)
                    {
                        positions.Add(i);
                        PathWay[i].unitMain = thisUnit;
                        if (i > 0)
                        {
                            PathWay[i - 1].unitMain = null;
                        }
                    }
                    break;
                }

            }


            thisUnitController.SetWaypoints(positions);

            if (!target)
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
        ComputerUnitPathAction();
        ComputerUnitCastleAction();
        

    }

    #region Computer Unit Action

    public void ComputerUnitPathAction()
    {
        //select units
        for (int i = 0; i <= pathLenght; i++)
        {
            if (PathWay[i].unitMain == null)
            {
                continue;
            }
            if (PathWay[i].unitMain.GetComponent<UnitControler>().IsThisPlayerUnit())
            {
                continue;
            }

            var thisUnit = PathWay[i].unitMain;
            var thisUnitController = thisUnit.GetComponent<UnitControler>();
            var thisUnitAttackReach = thisUnitController.ReturnAttackReach();
            var thisUnitMoveDistance = thisUnitController.ReturnMovmeDistance();

            //Unit attack
            bool target = false;
            for (int ii = 1; ii <= thisUnitAttackReach; ii++)
            {
                if (i - ii < 0)
                {
                    UnitAttack(thisUnitController, PlayerCastle.castle.GetComponent<CastleStats>());
                    target = true;
                    break;
                }
                if (PathWay[i - ii].unitMain == null)
                {
                    continue;
                }
                if (!PathWay[i - ii].unitMain.GetComponent<UnitControler>().IsThisPlayerUnit())
                {
                    continue;
                }
                UnitAttack(thisUnitController, PathWay[i - ii].unitMain.GetComponent<UnitControler>());
                target = true;
                break;
            }



            //Unit Move
            List<int> positions = new List<int>();
            PathWay[i].unitMain = thisUnit;
            for (int ii = 1; ii <= thisUnitMoveDistance; ii++)
            {
                if (i - ii < 0)
                {
                    break;
                }
                if (PathWay[i - ii].unitMain != null)
                {
                    var nextUnitOnPathController = PathWay[i - ii].unitMain.GetComponent<UnitControler>();
                    if ((nextUnitOnPathController.IsThisPlayerUnit()) && (nextUnitOnPathController.ReturnHp() - thisUnitController.ReturnDamage() <= 0))
                    {
                        positions.Add(i - ii);
                        PathWay[i - ii].unitMain = thisUnit;
                        PathWay[i - ii + 1].unitMain = null;
                        continue;
                    }
                    break;
                }
                if (PathWay[i - ii].unitMain == null)
                {
                    positions.Add(i - ii);
                    PathWay[i - ii].unitMain = thisUnit;
                    PathWay[i - ii + 1].unitMain = null;
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

    public void ComputerUnitCastleAction()
    {
        if (ComputerCastle.jednostka != null)
        {
            var thisUnit = ComputerCastle.jednostka;
            var thisUnitController = thisUnit.GetComponent<UnitControler>();
            var thisUnitAttackReach = thisUnitController.ReturnAttackReach();
            var thisUnitMoveDistance = thisUnitController.ReturnMovmeDistance();

            //Unit Attack
            bool target = false;
            for (int i = 0; i < thisUnitAttackReach; i++)
            {

                if (i > pathLenght)
                {
                    UnitAttack(thisUnitController, PlayerCastle.castle.GetComponent<CastleStats>());
                    target = true;
                    break;
                }
                if (PathWay[pathLenght - i].unitMain == null)
                {
                    continue;
                }
                if (!PathWay[pathLenght - i].unitMain.GetComponent<UnitControler>().IsThisPlayerUnit())
                {
                    continue;
                }
                UnitAttack(thisUnitController, PathWay[pathLenght - i].unitMain.GetComponent<UnitControler>());
                target = true;
                break;
            }



            //Unit Move

            List<int> position = new List<int>();
            for (int i = 0; i < thisUnitMoveDistance; i++)
            {
                if (PathWay[pathLenght - i].unitMain == null)
                {
                    position.Add(pathLenght - i);
                    PathWay[pathLenght - i].unitMain = thisUnit;
                    continue;
                }
                var nextUnitOnPathController = PathWay[pathLenght - i].unitMain.GetComponent<UnitControler>();
                if (!nextUnitOnPathController.IsThisPlayerUnit())
                {
                    break;
                }

                if (nextUnitOnPathController.IsThisPlayerUnit())
                {
                    if (nextUnitOnPathController.ReturnHp() - thisUnitController.ReturnDamage() <= 0)
                    {
                        position.Add(pathLenght - i);
                        PathWay[pathLenght - i].unitMain = thisUnit;
                    }
                    break;
                }
            }

            thisUnitController.SetWaypoints(position);
            if (!target)
            {
                thisUnitController.MoveAction();
            }
            if ((position?.Count() ?? 0) > 0)
            {
                ComputerCastle.jednostka = null;
            }
        }

    }
    #endregion
}
