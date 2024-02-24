using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static PathControler;
using static HexCoordinates;
using static DrogaClass;

public class PathGenerator : MonoBehaviour
{
    [SerializeField]

    public static PathGenerator Instance;

    
    [SerializeField] private float radius = 1;

    private void Awake()
    {
        Instance = this;
    }

    public List<Droga> GetNewDroga()
    {
        bool isThereNextTile = true;
        List<Droga> toReturn = new List<Droga>();
        List<GameObject> controlLista = new List<GameObject>();
        GameObject start = shotColliders(PathControler.Instance.PlayerCastle.castle.transform.position).First();
        Vector3 startcordy = start.transform.position;
        startcordy.y += 0.13f;
        toReturn.Add(new Droga { coordinations = startcordy, unitMain = null, wantingUnit = null });
        controlLista.Add(start);
        int control = 0;
        List<GameObject> hits = new();
        while (isThereNextTile)
        {
            hits = shotColliders(controlLista.Last().transform.position);
            foreach (var item in hits)
            {

                if (!controlLista.Contains(item))
                {
                    controlLista.Add(item);
                    Vector3 cordy = item.transform.position;
                    cordy.y += 0.13f;
                    toReturn.Add(new Droga { coordinations = cordy, unitMain = null, wantingUnit = null });
                    continue;
                }



            }

            if (hits.Count < 2 || control > 100)
            {
                isThereNextTile = false;
            }



            control++;

        }
        return toReturn;

    }

    private List<GameObject> shotColliders(Vector3 center)
    {
        List<GameObject> toReturn = new List<GameObject>();
        Collider[] hitColliders = Physics.OverlapSphere(center, radius, GameManager.instance.layerMask);
        foreach (var item in hitColliders)
        {
            toReturn.Add(item.gameObject);
        }



        return toReturn;

    }

   

}


