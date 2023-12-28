using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static CastleClass;
using Random = System.Random;
using static EnemySpawnClass;
using UnityEngine.UI;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine.EventSystems;

public class SpawnerScript : MonoBehaviour
{
    public static SpawnerScript instance;
    private Coroutine _SpawnerCoroutine;
    public bool playerRemovedCard = false;

    [SerializeField] public SpawmentListScriptableObject WhatEnemyCanSpawn;

    private void Awake()
    {
        instance = this;
    }

    [SerializeField] private Color _SelectedCard;
    private Color _DefaultCardColor = Color.white;

    //public void SpawnMyUnit(UnitScriptableObjects card) // tu jest stary spawn na jedną drogę
    //{
    //    if (GetComponent<GameManager>().CanPlayerMove())
    //    {
    //        var gdzie = GetComponent<PathControler>().PlayerCastle;
    //        if (gdzie.jednostka == null)
    //        {
    //            float x = gdzie.castle.transform.position.x;
    //            float y = 0.5f;
    //            float z = gdzie.castle.transform.position.z;

    //            GameObject spawnedUnit = SpawnObjectAtLocation(x, y, z, 90, card.unit);
    //            PutToList(spawnedUnit, gdzie);
    //            spawnedUnit.GetComponent<UnitControler>().SetSO(card);
    //        }
    //        else
    //        {
    //            Debug.Log("Miejsce zaj�te");
    //        }

    //    }
    //}

    public void SpawnMyUnit(GameObject karta, UnitScriptableObjects stats)
    {
        if (_SpawnerCoroutine != null)
        {
            StopCoroutine(_SpawnerCoroutine);
            karta.GetComponent<Image>().color = _DefaultCardColor;
        }
        else
        {
            _SpawnerCoroutine = StartCoroutine(SelectPathToSpawn(karta, stats));
        }
    }

    private IEnumerator SelectPathToSpawn(GameObject karta, UnitScriptableObjects stats)
    {
        karta.GetComponent<Image>().color = _SelectedCard;
        while (GameManager.instance.CanPlayerMove())
        {

            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                RaycastHit hit;

                if (Physics.Raycast(ray, out hit))
                {
                    if (!EventSystem.current.IsPointerOverGameObject())
                    {
                        Debug.Log("Hit object with tag: " + hit.collider.tag);

                        
                        
                        Gate thisGatePatch = GetPath(hit.collider.tag);
                        if (thisGatePatch == null)
                        {
                            break;
                        }
                        if (CanSpawn(thisGatePatch, stats))
                        {
                            Vector3 rotation = thisGatePatch.path[0].position - thisGatePatch.path[1].position;
                            Vector3 spawn = thisGatePatch.path[0].position;
                            UnitControler newUnit = SpawnObjectAtLocation(spawn.x, spawn.y + 0.15f, spawn.z, rotation.y + 90f, stats.unit).GetComponent<UnitControler>();
                            newUnit.SetSO(stats);
                            newUnit.setMyGate(thisGatePatch);
                            thisGatePatch.path[0].unitMain = newUnit;
                            Economy.Instance.Purchase(stats.cost);
                            Destroy(karta);
                            CardManager.instance.RevomeCard(karta);
                            UIController.Instance.ArrangeCards();
                        }

                        
                    }
                    break;

                }

            }
            yield return null;


        }
        karta.GetComponent<Image>().color = _DefaultCardColor;
        _SpawnerCoroutine = null;
        yield return null;
    }

    private Gate GetPath(string tag)
    {
        foreach (var item in CastlesController.Instance.playerCastle.gates)
        {
            if (item.CompareTag(tag))
            {
                return item;
            }
        }

        return null;
    }

    private bool CanSpawn(Gate gate, UnitScriptableObjects stats)
    {
        if (gate.path[0].unitMain != null)
        {
            EconomyConditions.Instance.ThereIsAUnit(); //Tu można zrobić funckje która na środku ekranu pokazuje tekst "HEX IS OCCUPIED"
            return false;
        }
        if (!Economy.Instance.CanIBuy(stats.cost))
        {
            EconomyConditions.Instance.NotEnoughCash(); //Tu można zrobić funckje która na środku ekranu pokazuje tekst "YOU CAN'T AFFORD IT / NOT ENOUGH FUNDS / NOT YOUR TURN"
        }

        return true;
    }

    public void SetRemoved(bool value)
    {
        playerRemovedCard = value;
    }

    public void SpawnEnemyUnit(int number)
    {
        

    }

    public void EnemyCheckSpawn()
    {
        

    }


    private GameObject SpawnObjectAtLocation(float posX, float posY, float posZ, float rota, GameObject spawn)
    {

        GameObject newObject;
        newObject = Instantiate(spawn, new Vector3(posX, posY, posZ), transform.rotation);
        newObject.transform.Rotate(0, rota, 0);
        newObject.transform.SetParent(GameObject.Find("Grid").transform, false);
        return newObject;

    }
    public void PutToList(GameObject unit, Castleee miejsce)
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
