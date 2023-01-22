using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static CastleClass;

public class SpawnerScript : MonoBehaviour
{
    public GameObject gigaPrefab;
    public GameObject EvilGigaPrefab;
    

    private void Start()
    {
        
    }

    public void SpawnMyUnit()
    {
        if (GetComponent<GameManager>().CanPlayerMove())
        { 
            var gdzie = GetComponent<PatchControler>().PlayerCastle;
            if (gdzie.jednostka==null)
            {
                float x = gdzie.castle.transform.position.x;
                float y = 0.5f;
                float z = gdzie.castle.transform.position.z;

                GameObject putToList = SpawnObjectAtLocation(x, y, z, 90,gigaPrefab);
                PutToList(putToList, gdzie);
            }
            else
            {
                Debug.Log("Miejsce zajête");
            }
        }
    }

   
    public void SpawnEnemyUnit()
    {
        if (GetComponent<GameManager>().CanComputerMove())
        {
            var gdzie = GetComponent<PatchControler>().ComputerCastle;
            if (gdzie.jednostka == null)
            {
                float x = gdzie.castle.transform.position.x;
                float y = 0.5f;
                float z = gdzie.castle.transform.position.z;

                GameObject putToList = SpawnObjectAtLocation(x, y, z, -90, EvilGigaPrefab);
                PutToList(putToList, gdzie);
            }
            else
            {
                Debug.Log("Miejsce zajête");
            }
        }
    }

    private GameObject SpawnObjectAtLocation(float posX, float posY, float posZ, float rota, GameObject spawn)
    {

        GameObject newObject;
        newObject = Instantiate(spawn, new Vector3(posX, posY, posZ), transform.rotation);
        newObject.transform.Rotate(0, rota, 0);
        newObject.transform.SetParent(GameObject.Find("Grid").transform, false);
        return newObject;

    }
    public void PutToList(GameObject unit, Castle miejsce)
    {
        miejsce.jednostka = unit;
    }
}
