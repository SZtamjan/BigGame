using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class PatchControler : MonoBehaviour
{
    private Vector3 offset=new(0.86f, 1, 0.75f);
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

    public int WielkoscListy;


    void Start()
    {


        PatchGenerator();
        WielkoscListy=drogaList.Count-1;




        //testo = hexGrid.GetNeighborsFor(cordy);
        //testDroga = new Droga() { Coordinations = TrueCoorde(testo[0]), jednostka = null };
        //drogaList.Add(testDroga);

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

        } while (warunek);
    }

    Vector3 TrueCoorde(Vector3 wektor)
    {
        if (wektor.z % 2==0)
        {
            wektor.x *= offset.x;
        }
        else
        {
            wektor.x = (wektor.x*offset.x)-(offset.x/2);

        }
        wektor.y = 0.13f;
        wektor.z *= offset.z;
        return wektor;


    }


    public void PlayerUnitMove()
    {
        
        for (int i = WielkoscListy; i>= 0; i--)
        {
            

            if ((drogaList[i].jednostka != null ) && (drogaList[i].jednostka.GetComponent<UnitStatistic>().ReturnattackplayersUnit()))
            {
                Debug.Log("dzia³a " + i);
                var unit = drogaList[i].jednostka.GetComponent<UnitStatistic>();
                GameObject enemy = PlayerUnitCheckRange(unit, i);

                if ((i + unit.ReturnattackReach()> WielkoscListy))
                {
                    PlayerUnitAttack(unit);
                    Debug.Log("Bije z pozycji " + i);
                }
                else if ((enemy != null)&&(!enemy.GetComponent<UnitStatistic>().ReturnattackplayersUnit()))
                {
                    Debug.Log("Bije z pozycji " + i);
                    PlayerUnitAttack(unit,enemy);
                }
                else if (enemy != null && enemy.GetComponent<UnitStatistic>().ReturnattackplayersUnit())
                {
                    Debug.Log("czekam na pozycji " + i);
                }
                else
                {
                    drogaList[i + 1].jednostka = drogaList[i].jednostka;
                   
                    PlayerMovment(drogaList[i + 1].Coordinations, drogaList[i].jednostka);
                    drogaList[i].jednostka = null;
                }

            }
        }
    }

    void PlayerUnitAttack(UnitStatistic unit)
    {
        Debug.Log("Jednostka zadaje "+unit.ReturnDamage()+" obrarzeñ zamkowi");
    }
    void PlayerUnitAttack(UnitStatistic unit,GameObject target)
    {
        Debug.Log("Jednostka zadaje " + unit.ReturnDamage() + " obrarzeñ jednostce");
    }

    void PlayerMovment(Vector3 coordinations, GameObject jednostka)
    {
        jednostka.transform.position = coordinations;
    }

    GameObject PlayerUnitCheckRange(UnitStatistic unit, int position)
    {
        if (position + 1>drogaList.Count-1)
        {
            return null;
        }
        for (int i = position+1 ; i <= position+unit.ReturnattackReach(); i++)
        {
            if (drogaList[i]!=null)
            {
                return drogaList[i].jednostka;
            }
        }

        return null;
    }


    // Update is called once per frame
    void Update()
    {
        
    }

    

}
