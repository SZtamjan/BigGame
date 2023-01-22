using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static CastleClass;
using static DrogaClass;

public class PatchControler : MonoBehaviour
{

    [Header("Starting ")]

    [SerializeField] public Castle PlayerCastle;
    [SerializeField] public Castle ComputerCastle;

    [SerializeField] public static List<Droga> pathway = new List<Droga>();
    [SerializeField] public List<Droga> pathwayDebug = new List<Droga>();

    public int pathLenght;

    public void StartPath()
    {

        pathwayDebug = pathway;
        pathLenght = pathway.Count - 1;
    }

    void Start()
    {




    }


    #region Computer Unit Movs
    public void ComputerUnitMove()
    {
        for (int i = 0; i <= pathLenght; i++) // movment for path
        {

            if ((pathway[i].jednostka != null) && (!pathway[i].jednostka.GetComponent<UnitControler>().IsThisPlayerUnit()))
            {
                GameObject unit = pathway[i].jednostka;
                var unitStatisctis = unit.GetComponent<UnitControler>();
                GameObject enemy = ComputerUnitCheckAttackReach(unitStatisctis, i);



                if ((enemy != null) && (enemy.GetComponent<UnitControler>().IsThisPlayerUnit()))
                {
                    Debug.Log("Enemy w Moj¹");
                    UnitAttack(unit, enemy);
                }

                else if ((i - unitStatisctis.ReturnattackReach() < 0))
                {
                    Debug.Log("Enemy w Zamek");
                    UnitAttack(unit);

                }

                int movmentDistance = ComputerUnitMovmentDistance(unitStatisctis, i);
                for (int ii = i; ii > i - movmentDistance; ii--)
                {

                    Vector3 nextTile;
                    pathway[ii - 1].jednostka = pathway[ii].jednostka;
                    if (ii - 1 == 0)
                    {
                        nextTile = PlayerCastle.castle.transform.position;
                    }
                    else
                    {
                        nextTile = pathway[ii - 2].Coordinations;
                    }

                    UnitMovment(pathway[ii - 1].Coordinations, pathway[ii].jednostka, nextTile);
                    pathway[ii].jednostka = null;
                }





            }
        }
        // movment for castle
        if (ComputerCastle.jednostka != null)
        {
            if (pathway.Last().jednostka != null)
            {
                if (pathway.Last().jednostka.GetComponent<UnitControler>().IsThisPlayerUnit())
                {
                    UnitAttack(ComputerCastle.jednostka, pathway.Last().jednostka);
                }


            }
            if (pathway.Last().jednostka == null || pathway.Last().jednostka.GetComponent<UnitControler>().ReturnHp() <= 0)
            {
                UnitCastleMovment(ComputerCastle, pathway.Last(), pathway[pathLenght - 1].Coordinations);

            }

        }



    }


    int ComputerUnitMovmentDistance(UnitControler unit, int position)
    {
        int movementDistance = unit.ReturnMovmentDistance();

        int freeDistance = movementDistance;

        for (int i = movementDistance; i >= 1; i--)
        {

            if (0 > position - i)
            {
                freeDistance--;
            }
            else if (pathway[position - i].jednostka != null)
            {
                if (pathway[position - i].jednostka.GetComponent<UnitControler>().ReturnHp() > 0)
                {
                    freeDistance = i - 1;
                }

            }

        }


        return freeDistance;
    }

    GameObject ComputerUnitCheckAttackReach(UnitControler unit, int position)
    {
        if (position == 0)
        {
            return null;
        }
        for (int i = position - 1; i >= position - unit.ReturnattackReach(); i--)
        {
            if (position - 1 < 0)
            {
                return null;
            }
            if (pathway[i].jednostka != null)
            {
                return pathway[i].jednostka;
            }
        }

        return null;
    }

    #endregion


    #region Player Units Movs 

    public void PlayerUnitMove()
    {

        for (int i = pathLenght; i >= 0; i--) // movs from path
        {


            if ((pathway[i].jednostka != null) && (pathway[i].jednostka.GetComponent<UnitControler>().IsThisPlayerUnit()))
            {

                GameObject unit = pathway[i].jednostka;
                var unitStatisctis = unit.GetComponent<UnitControler>();
                GameObject enemy = PlayerUnitCheckAttackReach(unitStatisctis, i);


                if ((enemy != null) && (!enemy.GetComponent<UnitControler>().IsThisPlayerUnit()))
                {
                    Debug.Log("Ja w Enemy");
                    UnitAttack(unit, enemy);
                }
                else if ((i + unitStatisctis.ReturnattackReach() > pathLenght))
                {
                    Debug.Log("Ja w Zamek");
                    UnitAttack(unit);

                }

                int movmentDistance = PlayerUnitMovmentDistance(unitStatisctis, i);
                for (int ii = i; ii < i + movmentDistance; ii++)
                {


                    pathway[ii + 1].jednostka = pathway[ii].jednostka;
                    Vector3 nextTile;
                    if (ii + 1 == pathLenght)
                    {
                        nextTile = ComputerCastle.castle.transform.position;
                    }
                    else
                    {
                        nextTile = pathway[ii + 2].Coordinations;
                    }
                    UnitMovment(pathway[ii + 1].Coordinations, pathway[ii].jednostka, nextTile);
                    pathway[ii].jednostka = null;
                }




            }
        }
        if (PlayerCastle.jednostka != null)// movs from castle
        {
            if (pathway[0].jednostka != null)
            {
                if (!pathway[0].jednostka.GetComponent<UnitControler>().IsThisPlayerUnit())
                {
                    UnitAttack(PlayerCastle.jednostka, pathway[0].jednostka);
                }


            }
            if (pathway[0].jednostka == null || pathway[0].jednostka.GetComponent<UnitControler>().ReturnHp() <= 0)
            {
                UnitCastleMovment(PlayerCastle, pathway[0], pathway[1].Coordinations);

            }
        }

    }


    int PlayerUnitMovmentDistance(UnitControler unit, int position)
    {
        int movementDistance = unit.ReturnMovmentDistance();

        int freeDistance = movementDistance;

        for (int i = movementDistance; i >= 1; i--)
        {

            if (pathLenght < position + i)
            {
                freeDistance--;
            }
            else if (pathway[position + i].jednostka != null)
            {
                if (pathway[position + i].jednostka.GetComponent<UnitControler>().ReturnHp() > 0)
                {
                    freeDistance = i - 1;
                }

            }

        }


        return freeDistance;
    }

    GameObject PlayerUnitCheckAttackReach(UnitControler unit, int position)
    {
        if (position + 1 > pathLenght)
        {
            return null;
        }
        for (int i = position + 1; i <= position + unit.ReturnattackReach(); i++)
        {
            if (i > pathLenght)
            {
                return null;
            }
            if (pathway[i].jednostka != null)
            {
                return pathway[i].jednostka;
            }
        }

        return null;
    }

    #endregion


    #region Player and Computer / Universal method
    void UnitMovment(Vector3 coordinations, GameObject jednostka, Vector3 nextTile)
    {
        jednostka.GetComponent<UnitMove>().AddToDestination(coordinations, nextTile);
    }

    void UnitAttack(GameObject unit)
    {
        UnitControler unitStat = unit.GetComponent<UnitControler>();
        Debug.Log("Jednostka zadaje " + unitStat.ReturnDamage() + " obrarzeñ zamkowi");
    }
    void UnitAttack(GameObject unit, GameObject target)
    {

        UnitControler unitStats = unit.GetComponent<UnitControler>();
        Debug.Log("Jednostka zadaje " + unitStats.ReturnDamage() + " obrarzeñ jednostce");
        target.GetComponent<UnitControler>().DamageTaken(unitStats.ReturnDamage());
    }

    void UnitCastleMovment(Castle castle, Droga miejsce, Vector3 nextTile)
    {
        miejsce.jednostka = castle.jednostka;
        UnitMovment(miejsce.Coordinations, castle.jednostka, nextTile);
        castle.jednostka = null;

    }




    #endregion


    // Update is called once per frame
    void Update()
    {

    }



}
