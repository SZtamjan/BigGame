using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class PatchControler : MonoBehaviour
{
    private Vector3 offset = new(0.86f, 1, 0.75f);
    [System.Serializable]
    public class Droga
    {
        public Vector3 Coordinations;
        public GameObject jednostka;
    }

    [Header("Starting ")]
    [SerializeField]

    public HexGrid hexGrid;
    public GameObject StartHex;



    [SerializeField]
    public List<Droga> drogaList = new List<Droga>();

    public int patchLenght;


    void Start()
    {
        PatchGenerator();
        patchLenght = drogaList.Count - 1;

    }
    void PatchGenerator()
    {
        List<Vector3Int> controlLista = new();

        Vector3Int cordy = StartHex.GetComponent<HexCoordinates>().GetHexCoords();
        Droga testDroga = new Droga() { Coordinations = TrueCoorde(cordy), jednostka = null };
        drogaList.Add(testDroga);
        controlLista.Add(cordy);

        List<Vector3Int> neighbours = new();
        bool warunek = true;
        int loopBreaker = 0;
        do
        {
            neighbours = hexGrid.GetNeighborsFor(controlLista.Last());
            foreach (var item in neighbours)
            {
                if (!controlLista.Contains(item))
                {
                    controlLista.Add(item);
                    drogaList.Add(new Droga { Coordinations = TrueCoorde(item), jednostka = null });

                }


            }
            if ((controlLista.Count > 2) && neighbours.Count == 1)
            {
                warunek = false;

            }
            loopBreaker++;
                if (loopBreaker>100)
            {
                warunek = false;
            }
        } while (warunek);
    }

    Vector3 TrueCoorde(Vector3 wektor)
    {
        if (wektor.z % 2 == 0)
        {
            wektor.x *= offset.x;
        }
        else
        {
            wektor.x = (wektor.x * offset.x) - (offset.x / 2);

        }
        wektor.y = 0.13f;
        wektor.z *= offset.z;
        return wektor;


    }


    #region Computer Unit Movs
    public void ComputerUnitMove()
    {
         for (int i = 0; i<=patchLenght; i++)
         {

            if ((drogaList[i].jednostka != null) && (!drogaList[i].jednostka.GetComponent<UnitStatistic>().IsThisPlayerUnit()))
            {
                var unit = drogaList[i].jednostka.GetComponent<UnitStatistic>();
                GameObject enemy = ComputerUnitCheckAttackReach(unit, i);



                if ((enemy != null) && (enemy.GetComponent<UnitStatistic>().IsThisPlayerUnit()))
                {

                    ComputerUnitAttack(unit, enemy);
                }

                else if ((i - unit.ReturnattackReach() < 0))
                {
                    ComputerUnitAttack(unit);

                }

                else if (enemy != null && !enemy.GetComponent<UnitStatistic>().IsThisPlayerUnit())
                {

                }

                else
                {
                    int movmentDistance = ComputerUnitMovmentDistance(unit, i);
                    for (int ii = i; ii > i - movmentDistance; ii--)
                    {


                        drogaList[ii - 1].jednostka = drogaList[ii].jednostka;

                        UnitMovment(drogaList[ii - 1].Coordinations, drogaList[ii].jednostka);
                        drogaList[ii].jednostka = null;
                    }


                }


            }
		 }
    }


    int ComputerUnitMovmentDistance(UnitStatistic unit, int position)
    {
        int movementDistance = unit.ReturnMovmentDistance();

        int freeDistance = movementDistance;
        for (int i = movementDistance; i >= 1; i--)
        {
            if (0 > position - i)
            {
                freeDistance--;
            }
            else if (drogaList[position - i].jednostka != null)
            {
                freeDistance = i - 1;
            }

        }


        return freeDistance;
    }

    void ComputerUnitAttack(UnitStatistic unit)
    {
        Debug.Log("Evil Jednostka zadaje " + unit.ReturnDamage() + " obrarzeñ zamkowi");
    }
    GameObject ComputerUnitCheckAttackReach(UnitStatistic unit, int position)
    {
        if (position == 0)
        {
            return null;
        }
        for (int i = position - 1; i >= position - unit.ReturnattackReach(); i--)
        {
            if (position - 1 <= 0)
            {
                return null;
            }
            if (drogaList[i].jednostka != null)
            {
                return drogaList[i].jednostka;
            }
        }

        return null;
    }

    void ComputerUnitAttack(UnitStatistic unit, GameObject target)
    {
        Debug.Log("Evil Jednostka zadaje " + unit.ReturnDamage() + " obrarzeñ jednostce");
    }

    #endregion

    








    #region Player Units Movs 

    public void PlayerUnitMove()
    {

        for (int i = patchLenght; i >= 0; i--)
        {


            if ((drogaList[i].jednostka != null) && (drogaList[i].jednostka.GetComponent<UnitStatistic>().IsThisPlayerUnit()))
            {

                var unit = drogaList[i].jednostka.GetComponent<UnitStatistic>();
                GameObject enemy = PlayerUnitCheckAttackReach(unit, i);


                if ((enemy != null) && (!enemy.GetComponent<UnitStatistic>().IsThisPlayerUnit()))
                {

                    PlayerUnitAttack(unit, enemy);
                }
                else if ((i + unit.ReturnattackReach() > patchLenght))
                {
                    PlayerUnitAttack(unit);

                }
                else if (enemy != null && enemy.GetComponent<UnitStatistic>().IsThisPlayerUnit())
                {

                }
                else
                {
                    int movmentDistance = PlayerUnitMovmentDistance(unit, i);
                    for (int ii = i; ii < i + movmentDistance; ii++)
                    {


                        drogaList[ii + 1].jednostka = drogaList[ii].jednostka;

                        UnitMovment(drogaList[ii + 1].Coordinations, drogaList[ii].jednostka);
                        drogaList[ii].jednostka = null;
                    }


                }

            }
        }
    }

    void PlayerUnitAttack(UnitStatistic unit)
    {
        Debug.Log("Moja Jednostka zadaje " + unit.ReturnDamage() + " obrarzeñ zamkowi");
    }
    void PlayerUnitAttack(UnitStatistic unit, GameObject target)
    {
        Debug.Log("Moja Jednostka zadaje " + unit.ReturnDamage() + " obrarzeñ jednostce");
    }


    int PlayerUnitMovmentDistance(UnitStatistic unit, int position)
    {
        int movementDistance = unit.ReturnMovmentDistance();

        int freeDistance = movementDistance;
        for (int i = movementDistance; i >= 1; i--)
        {
            if (patchLenght < position + i)
            {
                freeDistance--;
            }
            else if (drogaList[position + i].jednostka != null)
            {
                freeDistance = i - 1;
            }

        }


        return freeDistance;
    }

    GameObject PlayerUnitCheckAttackReach(UnitStatistic unit, int position)
    {
        if (position + 1 > patchLenght)
        {
            return null;
        }
        for (int i = position + 1; i <= position + unit.ReturnattackReach(); i++)
        {
            if (position + 1 > patchLenght)
            {
                return null;
            }
            if (drogaList[i].jednostka != null)
            {
                return drogaList[i].jednostka;
            }
        }

        return null;
    }

    #endregion

    #region Player and Computer / Universal method
    void UnitMovment(Vector3 coordinations, GameObject jednostka)
    {
        jednostka.transform.position = coordinations;
    }
    #endregion

    // Update is called once per frame
    void Update()
    {

    }



}
