using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static CastleClass;
using Random = System.Random;

public class SpawnerScript : MonoBehaviour
{
    public GameObject EvilGigaPrefab;
    public static SpawnerScript instance;
    [SerializeField] private List<UnitScriptableObjects> WhatEnemyCanSpawn;

    private void Awake()
    {
        instance=this;
    }
    

    public void SpawnMyUnit(UnitScriptableObjects card)
    {
        if (GetComponent<GameManager>().CanPlayerMove())
        {
            var gdzie = GetComponent<PatchControler>().PlayerCastle;
            if (gdzie.jednostka == null)
            {
                float x = gdzie.castle.transform.position.x;
                float y = 0.5f;
                float z = gdzie.castle.transform.position.z;

                GameObject spawnedUnit = SpawnObjectAtLocation(x, y, z, 90, card.unit);
                PutToList(spawnedUnit, gdzie);
                spawnedUnit.GetComponent<UnitControler>().SetSO(card);
            }
            else
            {
                Debug.Log("Miejsce zajête");
            }

        }
    }


    public void SpawnEnemyUnit()
    {
        int number = GetRandomInt(WhatEnemyCanSpawn.Count);

        var gdzie = GetComponent<PatchControler>().ComputerCastle;
        if (gdzie.jednostka == null)
        {
            float x = gdzie.castle.transform.position.x;
            float y = 0.5f;
            float z = gdzie.castle.transform.position.z;

            GameObject putToList = SpawnObjectAtLocation(x, y, z, -90, WhatEnemyCanSpawn[number].unit);
            PutToList(putToList, gdzie);
        }
        else
        {
            Debug.Log("Miejsce zajête");
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

    private int GetRandomInt(int max)
    {
        if (max == 1) return 1;

        Random rnd = new Random();
        return rnd.Next(max);
    }
}
