using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static PatchControler;
using static HexCoordinates;
using static DrogaClass;

public class PathGenerator : MonoBehaviour
{
    [SerializeField]
    public HexGrid hexGrid;
    public GameObject StartHex;
    private Vector3 offsets = offSet;

    public void PatchGenerator()
    {
        List<Vector3Int> controlLista = new();

        Vector3Int cordy = StartHex.GetComponent<HexCoordinates>().GetHexCoords();
        Droga testDroga = new Droga() { coordinations = TrueCoorde(cordy), unit = null };
        PathWay.Add(testDroga);
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
                    PathWay.Add(new Droga { coordinations = TrueCoorde(item), unit = null });

                }


            }
            if ((controlLista.Count > 2) && neighbours.Count == 1)
            {
                warunek = false;

            }
            loopBreaker++;
            if (loopBreaker > 1000)
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
            wektor.x *= offsets.x;
        }
        else
        {
            wektor.x = (wektor.x * offsets.x) - (offsets.x / 2);

        }
        wektor.y = 0.13f;
        wektor.z *= offsets.z;
        return wektor;


    }

}


