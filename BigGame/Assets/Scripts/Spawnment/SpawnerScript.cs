using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SpawnerScript : MonoBehaviour
{
    public GameObject gigaPrefab;
    

    public void ButtonClick()
    {
        if (CheckTurn())
        {
            var gdzie = GetComponent<PatchControler>().drogaList.First();
            if (gdzie.jednostka == null)
            {
                float x = gdzie.Coordinations.x;
                float y = 0.13f;
                float z = gdzie.Coordinations.z;

                GameObject putToList = SpawnObjectAtLocation(x, y, z, 90);
                PutToList(putToList, gdzie);
            }
            else
            {
                Debug.Log("MIejsce zajête");
            }


        }
        else
        {
            Debug.Log("Teraz jest kolej Przeciwnika");
        }

    }
    
    
    public void EvilButtonClick()
    {
        if (!CheckTurn())
        {
            var gdzie = GetComponent<PatchControler>().drogaList.Last();
            if (gdzie.jednostka == null)
            {
                float x = gdzie.Coordinations.x;
                float y = 0.13f;
                float z = gdzie.Coordinations.z;

                GameObject putToList = SpawnObjectAtLocation(x, y, z, -90);
                PutToList(putToList, gdzie);
            }
            else
            {
                Debug.Log("Miejsce zajête");
            }
        }
        else
        {
            Debug.Log("Teraz jest kolej gracza");
        }
    }
    public void PutToList(GameObject unit, PatchControler.Droga miejsce)
    {
        miejsce.jednostka = unit;

    }

    internal bool CheckTurn()
    {
        return GetComponent<GameManager>().ReturnTurn();
    }

    private GameObject SpawnObjectAtLocation(float posX, float posY, float posZ, float rota)
    {

        GameObject newObject;
        newObject = Instantiate(gigaPrefab, new Vector3(posX, posY, posZ), transform.rotation);
        newObject.transform.Rotate(0, rota, 0);
        newObject.transform.SetParent(GameObject.Find("Grid").transform, false);
        return newObject;

    }
}
