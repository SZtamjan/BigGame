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
using static UnityEngine.GraphicsBuffer;

public class PathControler : MonoBehaviour
{
    public static PathControler Instance;

    [Header("Starting ")]
    public Castle PlayerCastle;
    public Castle ComputerCastle;

    public static List<Droga> PathWay = new List<Droga>();
    public List<Droga> pathwayDebug = new List<Droga>();

    public List<Droga> TestowaDroga = new List<Droga>();

    public int pathLenght;

    private void Awake()
    {
        Instance = this;
    }
    public void StartNewPathWay()
    {
        PathWay = null;
        PathWay = new List<Droga>();
    }
    public void StartPath()
    {
        PathWay = PathGenerator.Instance.GetNewDroga();
        pathwayDebug = PathWay;
        pathLenght = PathWay.Count - 1;

        
    }


    #region Universal Attack Actions
    public void UnitAttack(UnitControler thisUnit, UnitControler targetUnit)
    {
        if (targetUnit != null)
        {
            thisUnit.SetTargetToAttack(targetUnit);
            targetUnit.HiddenDamageTaken(thisUnit.ReturnDamage());
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
        PlayerUnitPathAttack();
        PlayerUnitCastleAttack();

        PlayerUnitPathWalk();
        PlayerUnitCastleWalk();
    }

    #region Player Units Actions


    private void PlayerUnitPathAttack()
    {
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

            if (thisUnitController.ReturnHiddenHp() <= 0)
            {
                continue;
            }

            var thisUnitAttackReach = thisUnitController.ReturnAttackReach();

            for (int ii = 1; ii <= thisUnitAttackReach; ii++)
            {
                if (i + ii > pathLenght)
                {
                    UnitAttack(thisUnitController, ComputerCastle.castle.GetComponent<CastleStats>());

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
                if (PathWay[ii + i].unitMain.GetComponent<UnitControler>().ReturnHiddenHp() <= 0)
                {
                    continue;
                }

                UnitAttack(thisUnitController, PathWay[i + ii].unitMain.GetComponent<UnitControler>());

                break;
            }

        }
    }

    private void PlayerUnitPathWalk()
    {
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

            if (thisUnitController.ReturnHiddenHp() <= 0)
            {
                continue;
            }

            var thisUnitMoveDistance = thisUnitController.ReturnMovmeDistance();

            List<int> positions = new List<int>();
            PathWay[i].unitMain = thisUnit;
            for (int ii = 1; ii <= thisUnitMoveDistance; ii++)
            {
                if (i + ii > pathLenght)
                {
                    break;
                }

                if (PathWay[i + ii].unitMain == null)
                {
                    positions.Add(i + ii);
                    PathWay[i + ii].unitMain = thisUnit;
                    PathWay[i + ii - 1].unitMain = null;
                    continue;

                }

                if (PathWay[i + ii].unitMain != null)
                {
                    var nextUnitOnPathController = PathWay[i + ii].unitMain.GetComponent<UnitControler>();
                    if (!nextUnitOnPathController.IsThisPlayerUnit() && (nextUnitOnPathController.ReturnHiddenHp() <= 0))
                    {
                        positions.Add(i + ii);
                        PathWay[i + ii].unitMain = thisUnit;
                        PathWay[i + ii - 1].unitMain = null;
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

    private void PlayerUnitCastleAttack()
    {
        if (PlayerCastle.jednostka != null)
        {

            var thisUnit = PlayerCastle.jednostka;
            var thisUnitController = thisUnit.GetComponent<UnitControler>();
            var thisUnitAttackReach = thisUnitController.ReturnAttackReach();


            //Unit Attack

            for (int i = 0; i < thisUnitAttackReach; i++)
            {
                if (i > pathLenght)
                {
                    UnitAttack(thisUnitController, ComputerCastle.castle.GetComponent<CastleStats>());

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
                if (PathWay[i].unitMain.GetComponent<UnitControler>().ReturnHiddenHp() <= 0)
                {
                    continue;
                }
                UnitAttack(thisUnitController, PathWay[i].unitMain.GetComponent<UnitControler>());

                break;

            }
        }
    }

    private void PlayerUnitCastleWalk()
    {
        if (PlayerCastle.jednostka != null)
        {

            var thisUnit = PlayerCastle.jednostka;
            var thisUnitController = thisUnit.GetComponent<UnitControler>();
            var thisUnitMoveDistance = thisUnitController.ReturnMovmeDistance();



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

                if (PathWay[i].unitMain != null)
                {
                    var nextUnitOnPathController = PathWay[i].unitMain.GetComponent<UnitControler>();
                    if (!nextUnitOnPathController.IsThisPlayerUnit() && nextUnitOnPathController.ReturnHiddenHp() <= 0)
                    {
                        positions.Add(i);
                        PathWay[i].unitMain = thisUnit;
                        if (i > 0)
                        {
                            PathWay[i - 1].unitMain = null;
                            continue;
                        }
                        break;

                    }

                }

            }


            thisUnitController.SetWaypoints(positions);

            if (!thisUnitController.AmIDoingSomething())
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
        ComputerUnitPathAttack();
        ComputerUnitCastleAttack();

        ComputerUnitPathWalk();
        ComputerUnitCastleWalk();


    }

    #region Computer Unit Action

    private void ComputerUnitPathAttack()
    {
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

            if (thisUnitController.ReturnHiddenHp() <= 0)
            {
                continue;
            }

            var thisUnitAttackReach = thisUnitController.ReturnAttackReach();

            for (int ii = 1; ii <= thisUnitAttackReach; ii++)
            {
                if (i - ii < 0)
                {
                    UnitAttack(thisUnitController, PlayerCastle.castle.GetComponent<CastleStats>());

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
                if (PathWay[i - ii].unitMain.GetComponent<UnitControler>().ReturnHiddenHp() <= 0)
                {
                    continue;
                }
                UnitAttack(thisUnitController, PathWay[i - ii].unitMain.GetComponent<UnitControler>());

                break;
            }
        }
    }

    private void ComputerUnitPathWalk()
    {
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

            if (thisUnitController.ReturnHiddenHp() <= 0)
            {
                continue;
            }

            var thisUnitMoveDistance = thisUnitController.ReturnMovmeDistance();
            List<int> positions = new List<int>();
            PathWay[i].unitMain = thisUnit;
            for (int ii = 1; ii <= thisUnitMoveDistance; ii++)
            {
                if (i - ii < 0)
                {
                    break;
                }
                if (PathWay[i - ii].unitMain == null)
                {
                    positions.Add(i - ii);
                    PathWay[i - ii].unitMain = thisUnit;
                    PathWay[i - ii + 1].unitMain = null;
                    continue;
                }
                if (PathWay[i - ii].unitMain != null)
                {
                    var nextUnitOnPathController = PathWay[i - ii].unitMain.GetComponent<UnitControler>();
                    if ((nextUnitOnPathController.IsThisPlayerUnit()) && (nextUnitOnPathController.ReturnHiddenHp() <= 0))
                    {
                        positions.Add(i - ii);
                        PathWay[i - ii].unitMain = thisUnit;
                        PathWay[i - ii + 1].unitMain = null;
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

    private void ComputerUnitCastleAttack()
    {
        if (ComputerCastle.jednostka != null)
        {
            var thisUnit = ComputerCastle.jednostka;
            var thisUnitController = thisUnit.GetComponent<UnitControler>();
            var thisUnitAttackReach = thisUnitController.ReturnAttackReach();
            var thisUnitMoveDistance = thisUnitController.ReturnMovmeDistance();

            //Unit Attack

            for (int i = 0; i < thisUnitAttackReach; i++)
            {

                if (i > pathLenght)
                {
                    UnitAttack(thisUnitController, PlayerCastle.castle.GetComponent<CastleStats>());

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
                if (PathWay[pathLenght - i].unitMain.GetComponent<UnitControler>().ReturnHiddenHp() <= 0)
                {
                    continue;
                }
                UnitAttack(thisUnitController, PathWay[pathLenght - i].unitMain.GetComponent<UnitControler>());

                break;
            }
        }
    }

    private void ComputerUnitCastleWalk()
    {
        if (ComputerCastle.jednostka != null)
        {
            var thisUnit = ComputerCastle.jednostka;
            var thisUnitController = thisUnit.GetComponent<UnitControler>();
            var thisUnitMoveDistance = thisUnitController.ReturnMovmeDistance();


            List<int> positions = new List<int>();
            for (int i = 0; i < thisUnitMoveDistance; i++)
            {
                if (pathLenght - i < 0)
                {
                    break;
                }
                if (PathWay[pathLenght - i].unitMain == null)
                {
                    positions.Add(pathLenght - i);
                    PathWay[pathLenght - i].unitMain = thisUnit;
                    continue;
                }

                if (PathWay[pathLenght - i].unitMain != null)
                {
                    var nextUnitOnPathController = PathWay[pathLenght - i].unitMain.GetComponent<UnitControler>();
                    if (nextUnitOnPathController.IsThisPlayerUnit() && nextUnitOnPathController.ReturnHiddenHp() <= 0)
                    {
                        positions.Add(pathLenght - i);
                        PathWay[pathLenght - i].unitMain = thisUnit;
                        if (pathLenght - i < pathLenght)
                        {
                            PathWay[pathLenght - i + 1].unitMain = null;
                            continue;
                        }
                        break;

                    }

                }
            }

            thisUnitController.SetWaypoints(positions);

            if (!thisUnitController.AmIDoingSomething())
            {
                thisUnitController.MoveAction();

            }


            if ((positions?.Count() ?? 0) > 0)
            {
                ComputerCastle.jednostka = null;
            }
        }

    }

    #endregion
}
