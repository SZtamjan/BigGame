using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SpawnerScript : MonoBehaviour
{
    public GameObject gigaPrefab;
    public GameObject EvilGigaPrefab;

    public void ButtonClick()
    {
        if (GetComponent<GameManager>().CanPlayerMove())
        { 
            var gdzie = GetComponent<PatchControler>().drogaList.First();
            if (gdzie.jednostka==null)
            {
                float x = gdzie.Coordinations.x;
                float y = 0.13f;
                float z = gdzie.Coordinations.z;

                GameObject putToList = SpawnObjectAtLocation(x, y, z, 90,gigaPrefab);
                PutToList(putToList, gdzie);
            }
            else
            {
                Debug.Log("MIejsce zajête");
            }
        }
    }

    private GameObject SpawnObjectAtLocation(float posX, float posY, float posZ, float rota,GameObject spawn)
    {
        
        GameObject newObject;
        newObject = Instantiate(spawn, new Vector3(posX, posY, posZ), transform.rotation);
        newObject.transform.Rotate(0, rota, 0);
        newObject.transform.SetParent(GameObject.Find("Grid").transform, false);
        return newObject;

    }
    public void EvilButtonClick()
    {
        if (GetComponent<GameManager>().CanComputerMove())
        {
            var gdzie = GetComponent<PatchControler>().drogaList.Last();
            if (gdzie.jednostka == null)
            {
                float x = gdzie.Coordinations.x;
                float y = 0.13f;
                float z = gdzie.Coordinations.z;

                GameObject putToList = SpawnObjectAtLocation(x, y, z, -90, EvilGigaPrefab);
                PutToList(putToList, gdzie);
            }
            else
            {
                Debug.Log("Miejsce zajête");
            }
        }
    }
    public void PutToList(GameObject unit, PatchControler.Droga miejsce)
    {
        miejsce.jednostka = unit;

    }
}
