using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class PatchControler : MonoBehaviour
{
    private Vector3 offset=new(0.8659766f, 1, 0.75f);
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
    
   
    [Header("Testowe ")]
    [SerializeField]
    public List<Droga> drogaList = new List<Droga>();
    public List<Vector3Int> neighbours;

    void Start()
    {
        List<Vector3Int> controlLista = new();
        
        Vector3Int cordy = StartHex.GetComponent<HexCoordinates>().GetHexCoords();
        Droga testDroga = new Droga() { Coordinations = TrueCoorde(cordy), jednostka = null };
        drogaList.Add(testDroga);
        controlLista.Add(cordy);

        neighbours = new();
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
                    Debug.Log("1");
                }
                else
                {
                    
                    Debug.Log("2");
                }
                Debug.Log("3");
            }
            if ((controlLista.Count>2)&&neighbours.Count==1)
            {
                warunek = false;
                
            }

        } while (warunek);







        //testo = hexGrid.GetNeighborsFor(cordy);
        //testDroga = new Droga() { Coordinations = TrueCoorde(testo[0]), jednostka = null };
        //drogaList.Add(testDroga);

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
        wektor.y *= offset.y;
        wektor.z *= offset.z;
        return wektor;


    }

    // Update is called once per frame
    void Update()
    {
        
    }

    

}
