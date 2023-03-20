using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static CastleClass;
using static DrogaClass;

public class PatchControler : MonoBehaviour
{

    [Header("Starting ")]
    public Castle PlayerCastle;
    public Castle ComputerCastle;

    public static List<Droga> pathway = new List<Droga>();
    public List<Droga> pathwayDebug = new List<Droga>();

    public int pathLenght;

    public void StartPath()
    {
        pathwayDebug = pathway;
        pathLenght = pathway.Count - 1;
    }


    // ----------------------------------------- OLD CODE
    #region Move First Units
    //public void MoveFirstsUnits()
    //{
    //    FirstUnitsPathReservation();



    //    FirstUnitPlayerMove();

    //    FirstUnitComputerMove();

    //    FirtsUnitReservationClear();
    //}

    //public void FirstUnitsPathReservation()
    //{
    //    #region path reservation

    //    int? firstPlayerPos = FirstPlayerUnitPosition();
    //    int? firstEnemyPos = FirstComputerUnitPosition();

    //    if (firstPlayerPos != null)
    //    {
    //        int playerPos = firstPlayerPos ?? 0;
    //        var thisUnit = pathway[playerPos].unit;
    //        var moveDistance = thisUnit.GetComponent<UnitControler>().ReturnMovmentDistance();
    //        for (int i = 1; i <= moveDistance; i++)
    //        {
    //            if (firstPlayerPos + i > pathLenght)
    //            {
    //                break;
    //            }
    //            pathway[playerPos + i].wantingUnit = thisUnit;
    //            pathway[playerPos + i].holdPower = i;
    //        }
    //    }


    //    if (firstEnemyPos != null)
    //    {
    //        int enemyPos = firstEnemyPos ?? 0;
    //        var thisUnit = pathway[enemyPos].unit;
    //        var moveDistance = thisUnit.GetComponent<UnitControler>().ReturnMovmentDistance();
    //        for (int i = 1; i <= moveDistance; i++)
    //        {
    //            if (enemyPos - i < 0)
    //            {
    //                break;
    //            }
    //            if (pathway[enemyPos - i].holdPower <= i && pathway[enemyPos - i].holdPower != 0)
    //            {
    //                break;
    //            }
    //            pathway[enemyPos - i].wantingUnit = thisUnit;
    //            pathway[enemyPos - i].holdPower = i;
    //        }
    //    }


    //    #endregion
    //}

    //public void FirstUnitPlayerMove()
    //{
    //    int? firstPlayerPos = FirstPlayerUnitPosition();
    //    if (firstPlayerPos != null)
    //    {
    //        int playerPos = firstPlayerPos ?? 0;
    //        var thisUnit = pathway[playerPos].unit;
    //        var moveDistance = thisUnit.GetComponent<UnitControler>().ReturnMovmentDistance();
    //        for (int i = 1; i <= moveDistance; i++)
    //        {
    //            if (playerPos + i > pathLenght)
    //            {
    //                break;
    //            }
    //            if (pathway[playerPos + i].unit != null)
    //            {
    //                break;
    //            }
    //            if (pathway[playerPos + i].wantingUnit == thisUnit)
    //            {
    //                Vector3 nextTile;
    //                if (playerPos + i + 1 > pathLenght)
    //                {
    //                    nextTile = ComputerCastle.castle.transform.position;
    //                }
    //                else
    //                {
    //                    nextTile = pathway[playerPos + i + 1].coordinations;
    //                }
    //                thisUnit.GetComponent<UnitControler>().MoveUnit(pathway[playerPos + i].coordinations, nextTile);
    //                pathway[playerPos + i].unit = thisUnit;
    //                pathway[playerPos + i - 1].unit = null;

    //            }
    //        }

    //    }
    //}

    //public void FirstUnitComputerMove()
    //{
    //    int? firstEnemyPos = FirstComputerUnitPosition();
    //    if (firstEnemyPos != null)
    //    {
    //        int computerPos = firstEnemyPos ?? 0;
    //        var thisUnit = pathway[computerPos].unit;
    //        var moveDistance = thisUnit.GetComponent<UnitControler>().ReturnMovmentDistance();
    //        for (int i = 1; i <= moveDistance; i++)
    //        {
    //            if (computerPos - i < 0)
    //            {
    //                break;
    //            }
    //            if (pathway[computerPos - i].unit != null)
    //            {
    //                break;
    //            }
    //            if (pathway[computerPos - i].wantingUnit == thisUnit)
    //            {
    //                Vector3 nextTile;
    //                if (computerPos - i - 1 < 0)
    //                {
    //                    nextTile = PlayerCastle.castle.transform.position;
    //                }
    //                else
    //                {
    //                    nextTile = pathway[computerPos - i - 1].coordinations;
    //                }

    //                thisUnit.GetComponent<UnitControler>().MoveUnit(pathway[computerPos - i].coordinations, nextTile);
    //                pathway[computerPos - i].unit = thisUnit;
    //                pathway[computerPos - i + 1].unit = null;

    //            }

    //        }
    //    }
    //}

    //public void FirtsUnitReservationClear()
    //{
    //    foreach (var item in pathway)
    //    {
    //        item.wantingUnit = null;
    //        item.holdPower = 0;
    //    }

    //}

    #endregion

    //---------------------------------------------
    #region Rest Units Move 


    public void PlayerMoveUnitsOnPath()
    {
        int firstUnit = FirstPlayerUnitPosition() ?? 0;
        for (int i = firstUnit; i >= 0; i--)
        {
            var thisTile = pathway[i];
            if (thisTile.unit != null)
            {
                var thisUnit = thisTile.unit;
                var thisUnitController = thisUnit.GetComponent<UnitControler>();
                if (!thisUnitController.IsThisPlayerUnit())
                {
                    continue;
                }

                bool wasThereAttack = PlayerUnitAttack(i, thisTile, thisUnit, thisUnitController);

                float timeDelay = wasThereAttack ? 2.8f : 2.8f;

                StartCoroutine(PlayerActualMove(i, thisTile, thisUnit, thisUnitController, timeDelay));



            }

        }

    }
    IEnumerator PlayerActualMove(int i, Droga thisTile, GameObject thisUnit, UnitControler thisUnitController, float timeDelay)
    {


        var moveDistance = thisUnitController.ReturnMovmentDistance();
        Vector3 nextTile;

        for (int ii = 1; ii <= moveDistance; ii++)
        {
            if (i + ii > pathLenght)
            {
                break;
            }
            if (pathway[i + ii].unit != null)
            {
                break;
            }
            if (i + ii + 1 > pathLenght)
            {
                nextTile = ComputerCastle.castle.transform.position;
            }
            else
            {
                nextTile = pathway[i + ii + 1].coordinations;
            }


            pathway[i + ii - 1].unit = null;
            pathway[i + ii].unit = thisUnit;

            thisUnitController.MoveUnit(pathway[i + ii].coordinations, nextTile);

        }
        if (thisTile.unit == null)
        {
            yield return new WaitForSeconds(timeDelay);
            thisUnitController.playMove();
        }
        yield return null;
    }

    public bool PlayerUnitAttack(int i, Droga thisTile, GameObject thisUnit, UnitControler thisUnitController)
    {
        var attackReach = thisUnitController.ReturnattackReach();
        for (int ii = 1; ii <= attackReach; ii++)
        {
            if (i + ii > pathLenght)
            {
                Attack(thisUnitController, ComputerCastle.castle.GetComponent<CastleStats>());
                return true;

            }
            if (pathway[i + ii].unit == null)
            {
                continue;
            }
            if (pathway[i + ii].unit.GetComponent<UnitControler>().IsThisPlayerUnit())
            {
                continue;
            }

            var targetController = pathway[i + ii].unit.GetComponent<UnitControler>();
            Attack(thisUnitController, targetController);
            if (targetController.ReturnHp() <= 0)
            {
                pathway[i + ii].unit = null;
            }
            return true;

        }



        return false;
    }


    public void ComputerMoveUnitsOnPath()
    {
        int firstUnit = FirstComputerUnitPosition() ?? 0;
        for (int i = firstUnit; i <= pathLenght; i++)
        {
            var thisTile = pathway[i];
            if (thisTile.unit != null)
            {
                var thisUnit = thisTile.unit;
                var thisUnitController = thisUnit.GetComponent<UnitControler>();
                if (thisUnitController.IsThisPlayerUnit())
                {
                    continue;
                }

                bool wasThereAttack = ComputerUnitAttack(i, thisTile, thisUnit, thisUnitController);
                float timeDelay = wasThereAttack ? 2.8f : 2.8f;

                StartCoroutine(ComputerActualMove(i, thisTile, thisUnit, thisUnitController, timeDelay));

            }
        }

    }

    IEnumerator ComputerActualMove(int i, Droga thisTile, GameObject thisUnit, UnitControler thisUnitController, float timeDelay)
    {
        var moveDistance = thisUnitController.ReturnMovmentDistance();
        Vector3 nextTile;

        for (int ii = 1; ii <= moveDistance; ii++)
        {
            if (i - ii < 0)
            {
                break;
            }
            if (pathway[i - ii].unit != null)
            {
                break;
            }
            if (i - ii - 1 < 0)
            {
                nextTile = PlayerCastle.castle.transform.position;
            }
            else
            {
                nextTile = pathway[i - ii - 1].coordinations;
            }
            pathway[i - ii + 1].unit = null;
            pathway[i - ii].unit = thisUnit;

            thisUnitController.MoveUnit(pathway[i - ii].coordinations, nextTile);

        }
        if (thisTile.unit == null)
        {
            yield return new WaitForSeconds(timeDelay);
            thisUnitController.playMove();
        }
        yield return null;

    }


    #endregion

    #region move from Castle
    public void PlayerMoveFromCastle()
    {
        if (PlayerCastle.jednostka != null)
        {
            var thisUnit = PlayerCastle.jednostka;
            var thisUnitController = thisUnit.GetComponent<UnitControler>();
            if (pathway[0].unit == null)
            {

                pathway[0].unit = thisUnit;
                thisUnitController.MoveUnit(pathway[0].coordinations, pathway[1].coordinations);
                PlayerCastle.jednostka = null;
            }
            if (PlayerCastle.jednostka == null)
            {
                thisUnitController.playMove();
            }
        }
    }

    public void ComputerMoveFromCastle()
    {


        if (ComputerCastle.jednostka != null)
        {
            var thisUnit = ComputerCastle.jednostka;
            var thisUnitController = thisUnit.GetComponent<UnitControler>();
            if (pathway.Last().unit == null)
            {

                pathway.Last().unit = thisUnit;
                thisUnitController.MoveUnit(pathway[pathLenght].coordinations, pathway[pathLenght - 1].coordinations);
                ComputerCastle.jednostka = null;
            }
            if (ComputerCastle.jednostka == null)
            {
                thisUnitController.playMove();
            }
        }

    }

    #endregion

    int? FirstPlayerUnitPosition()
    {
        int? firstPos = null;
        for (int i = pathLenght; i >= 0; i--)
        {

            if (pathway[i].unit == null)
            {
                continue;
            }
            if (pathway[i].unit.GetComponent<UnitControler>().IsThisPlayerUnit())
            {
                firstPos = i;
                break;
            }
        }
        return firstPos;
    }

    int? FirstComputerUnitPosition()
    {
        int? FirstEnemyUnit = null;
        for (int i = 0; i <= pathLenght; i++)
        {
            if (pathway[i].unit == null)
            {
                continue;
            }
            if (!pathway[i].unit.GetComponent<UnitControler>().IsThisPlayerUnit())
            {
                FirstEnemyUnit = i;
                break;
            }
        }
        int? positions = FirstEnemyUnit;

        return positions;
    }

    #region Units Attack


    // stara funkcja mo¿e pos³u¿y jako karta do u¿ycia do zaakakowania poza tur¹
    #region 
    //public void PlayerUnitAttack()
    //{
    //    bool didSomeoneAttack = false;

    //    int firstUnit = FirstPlayerUnitPosition() ?? 0;
    //    for (int i = firstUnit; i >= 0; i--)
    //    {
    //        var thisTile = pathway[i];
    //        if (thisTile.unit != null)
    //        {
    //            var thisUnit = thisTile.unit;
    //            var thisUnitController = thisUnit.GetComponent<UnitControler>();

    //            for (int ii = 1; ii <= thisUnitController.ReturnattackReach(); ii++)
    //            {
    //                if (i + ii > pathLenght)
    //                {
    //                    Attack(thisUnitController, ComputerCastle.castle.GetComponent<CastleStats>());
    //                    didSomeoneAttack = true;
    //                    break;
    //                }
    //                if (pathway[i + ii].unit == null)
    //                {
    //                    continue;
    //                }
    //                if (pathway[i + ii].unit.GetComponent<UnitControler>().IsThisPlayerUnit())
    //                {
    //                    continue;
    //                }

    //                bool toClear = Attack(thisUnitController, pathway[i + ii].unit.GetComponent<UnitControler>());
    //                if (toClear)
    //                {
    //                    pathway[i + ii].unit = null;
    //                }
    //                didSomeoneAttack = true;
    //                break;

    //            }


    //        }

    //    }

    //    return didSomeoneAttack;
    //}
    #endregion

    public bool ComputerUnitAttack(int i, Droga thisTile, GameObject thisUnit, UnitControler thisUnitController)
    {
        var attackReach = thisUnitController.ReturnattackReach();
        for (int ii = 1; ii <= attackReach; ii++)
        {
            if (i - ii < 0)
            {
                Attack(thisUnitController, PlayerCastle.castle.GetComponent<CastleStats>());
                return true;
            }
            if (pathway[i - ii].unit == null)
            {
                continue;
            }
            if (!pathway[i - ii].unit.GetComponent<UnitControler>().IsThisPlayerUnit())
            {
                continue;
            }
            var targetController = pathway[i - ii].unit.GetComponent<UnitControler>();
            Attack(thisUnitController, targetController);
            if (targetController.ReturnHp() <= 0)
            {
                pathway[i - ii].unit = null;
            }
            return true;
        }


        return false;
    }

    private void Attack(UnitControler unit, CastleStats target) // atak w zamek
    {
        int damage = unit.ReturnDamage();
        unit.playAttack();
        target.DamageTaken(damage);

    }

    private bool Attack(UnitControler unit, UnitControler target) // atakw w jednostkê
    {
        int damage = unit.ReturnDamage();
        unit.playAttack();
        target.DamageTaken(damage);

        return target.ReturnHp() <= 0;
    }

    #endregion


    public void ComputerUnitPhaze()
    {
        StartCoroutine(ComputerUnitAction());
    }

    IEnumerator ComputerUnitAction()
    {

        ComputerMoveUnitsOnPath();
        ComputerMoveFromCastle();

        yield return null;
        GameManager.instance.UpdateGameState(GameManager.GameState.PlayerTurn);
        yield return null;
    }



    public void PlayerUnitPhase()
    {
        StartCoroutine(PlayerUnitsAction());
    }

    IEnumerator PlayerUnitsAction()
    {
        PlayerMoveUnitsOnPath();
        PlayerMoveFromCastle();

        yield return new WaitForSeconds(1f);
        GameManager.instance.UpdateGameState(GameManager.GameState.EnemyTurn);
        yield return null;
    }











    // Update is called once per frame
    void Update()
    {

    }



}
