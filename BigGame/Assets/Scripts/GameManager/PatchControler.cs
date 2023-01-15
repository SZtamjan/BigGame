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
    [System.Serializable]
    public class Castle
    {
        public GameObject castle;
        public GameObject jednostka=null;
    }

    [Header("Starting ")]
    [SerializeField]

    public HexGrid hexGrid;
    public GameObject StartHex;
    [SerializeField] public Castle PlayerCastle;
    [SerializeField] public Castle ComputerCastle;

    [SerializeField] public List<Droga> drogaList = new List<Droga>();
    

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
                if (loopBreaker>1000)
            {
                Debug.Log("coœ posz³o nie tak \n generowanie drogi siê zepsu³o");
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
         for (int i = 0; i<=patchLenght; i++) // movment for path
         {

            if ((drogaList[i].jednostka != null) && (!drogaList[i].jednostka.GetComponent<UnitStatistic>().IsThisPlayerUnit()))
            {
                GameObject unit = drogaList[i].jednostka;
                var unitStatisctis = unit.GetComponent<UnitStatistic>();
                GameObject enemy = ComputerUnitCheckAttackReach(unitStatisctis, i);



                if ((enemy != null) && (enemy.GetComponent<UnitStatistic>().IsThisPlayerUnit()))
                {
                    Debug.Log("Enemy w Moj¹");
                    UnitAttack(unit, enemy);
                }

                else if ((i - unitStatisctis.ReturnattackReach() < 0))
                {
                    Debug.Log("Enemy w Zamek");
                    UnitAttack(unit);

                }
                else
                {
                    int movmentDistance = ComputerUnitMovmentDistance(unitStatisctis, i);
                    for (int ii = i; ii > i - movmentDistance; ii--)
                    {

                        Vector3 nextTile;
                        drogaList[ii - 1].jednostka = drogaList[ii].jednostka;
                        if (ii-1==0)
                        {
                            nextTile = PlayerCastle.castle.transform.position;
                        }
                        else
                        {
                            nextTile=drogaList[ii-2].Coordinations;
                        }

                        UnitMovment(drogaList[ii - 1].Coordinations, drogaList[ii].jednostka,nextTile);
                        drogaList[ii].jednostka = null;
                    }


                }


            }
		 }
        // movment for castle
        if (ComputerCastle.jednostka!=null)
        {
            if (drogaList.Last().jednostka != null)
            {
                if (drogaList.Last().jednostka.GetComponent<UnitStatistic>().IsThisPlayerUnit())
                {
                    UnitAttack(ComputerCastle.jednostka, drogaList.Last().jednostka);
                }


            }
            if (drogaList.Last().jednostka == null || drogaList.Last().jednostka.GetComponent<UnitStatistic>().ReturnHp() <= 0)
            {
                UnitCastleMovment(ComputerCastle, drogaList.Last(), drogaList[patchLenght-1].Coordinations);

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

    GameObject ComputerUnitCheckAttackReach(UnitStatistic unit, int position)
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
            if (drogaList[i].jednostka != null)
            {
                return drogaList[i].jednostka;
            }
        }

        return null;
    }

    #endregion


    #region Player Units Movs 

    public void PlayerUnitMove()
    {

        for (int i = patchLenght; i >= 0; i--) // movs from path
        {


            if ((drogaList[i].jednostka != null) && (drogaList[i].jednostka.GetComponent<UnitStatistic>().IsThisPlayerUnit()))
            {

                GameObject unit = drogaList[i].jednostka;
                var unitStatisctis = unit.GetComponent<UnitStatistic>();
                GameObject enemy = PlayerUnitCheckAttackReach(unitStatisctis, i);


                if ((enemy != null) && (!enemy.GetComponent<UnitStatistic>().IsThisPlayerUnit()))
                {
                    Debug.Log("Ja w Enemy");
                    UnitAttack(unit, enemy);
                }
                else if ((i + unitStatisctis.ReturnattackReach() > patchLenght))
                {
                    Debug.Log("Ja w Zamek");
                    UnitAttack(unit);

                }                
                
                    int movmentDistance = PlayerUnitMovmentDistance(unitStatisctis, i);
                    for (int ii = i; ii < i + movmentDistance; ii++)
                    {


                        drogaList[ii + 1].jednostka = drogaList[ii].jednostka;
                        Vector3 nextTile;
                        if (ii+1==patchLenght)
                        {
                            nextTile = ComputerCastle.castle.transform.position;
                        }
                        else
                        {
                            nextTile = drogaList[ii + 2].Coordinations;
                        }
                        UnitMovment(drogaList[ii + 1].Coordinations, drogaList[ii].jednostka, nextTile);
                        drogaList[ii].jednostka = null;
                    }


                

            }
        }
        if (PlayerCastle.jednostka != null)// movs from castle
        {
            if (drogaList[0].jednostka != null)
            {
                if (!drogaList[0].jednostka.GetComponent<UnitStatistic>().IsThisPlayerUnit())
                {
                    UnitAttack(PlayerCastle.jednostka, drogaList[0].jednostka);
                }


            }
            if (drogaList[0].jednostka == null || drogaList[0].jednostka.GetComponent<UnitStatistic>().ReturnHp()<=0)
            {
                UnitCastleMovment(PlayerCastle, drogaList[0], drogaList[1].Coordinations);

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
        target.GetComponent<UnitStatistic>().DamageTaken(unit.ReturnDamage());
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
            if (i > patchLenght)
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
    void UnitMovment(Vector3 coordinations, GameObject jednostka, Vector3 nextTile)
    {
        jednostka.GetComponent<UnitMove>().AddToDestination(coordinations, nextTile);
    }

    void UnitAttack(GameObject unit)
    {
        UnitStatistic unitStat = unit.GetComponent<UnitStatistic>();
        Debug.Log("Moja Jednostka zadaje " + unitStat.ReturnDamage() + " obrarzeñ zamkowi");
    }
    void UnitAttack(GameObject unit, GameObject target)
    {
        
        UnitStatistic unitStats = unit.GetComponent<UnitStatistic>();
        Debug.Log("Moja Jednostka zadaje " + unitStats.ReturnDamage() + " obrarzeñ jednostce");
        target.GetComponent<UnitStatistic>().DamageTaken(unitStats.ReturnDamage());        
    }

    void UnitCastleMovment(Castle castle, Droga miejsce,Vector3 nextTile)
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
