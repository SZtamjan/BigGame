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
using Economy.EconomyActions;
using static UnitsStatsClass;

public class UnitSpawner : EconomyOperations
{
    public static UnitSpawner instance;
    private Coroutine _SpawnerCoroutine;
    public bool playerRemovedCard = false;

    [SerializeField] public SpawmentListScriptableObject WhatEnemyCanSpawn;

    private void Awake()
    {
        instance = this;
    }
    
    //[SerializeField] private Color _SelectedCard;
    //private Color _DefaultCardColor = Color.white;
    public GameObject _selectedCard;

    public GameObject SelectedCardProp
    {
        get => _selectedCard;
        set => _selectedCard = value;
    }

    public void SpawnMyUnit(GameObject karta, CardScriptableObject stats)
    {
        if (_SpawnerCoroutine != null)
        {
            StopCoroutine(_SpawnerCoroutine);
            _SpawnerCoroutine=null;
            karta.GetComponent<Image>().color = CardManager.instance.defaultCardColor;
        }
        else
        {
            UIController.Instance.SwitchTrashcanActive();
            _SpawnerCoroutine = StartCoroutine(SelectPathToSpawn(karta, stats));
        }
    }

    private IEnumerator SelectPathToSpawn(GameObject karta, CardScriptableObject stats)
    {
        _selectedCard = karta;
        karta.GetComponent<Image>().color = CardManager.instance.selectedCardColor;
        while (GameManager.instance.CanPlayerMove())
        {
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                
                if (!Physics.Raycast(ray, out hit))
                {
                    break;
                }
                if (EventSystem.current.IsPointerOverGameObject())
                {
                    break;
                }

                Debug.Log("Hit object with tag: " + hit.collider.tag);

                Gate thisGatePatch = GetPath(hit.collider.tag);
                if (thisGatePatch == null)
                {
                    break;
                }

                if (CheckSpawnConditions(thisGatePatch, stats))
                {
                    Vector3 rotation = thisGatePatch.path[0].position - thisGatePatch.path[1].position;
                    Vector3 spawn = thisGatePatch.path[0].position;
                    UnitsStats unitStats = (UnitsStats)stats.GetStats();

                    UnitControler newUnit = SpawnObjectAtLocation(spawn.x, spawn.y + 0.15f, spawn.z, rotation.y + 90f, unitStats.unit).GetComponent<UnitControler>();
                    newUnit.SetSO(unitStats);
                    newUnit.setMyGate(thisGatePatch);
                    thisGatePatch.path[0].unitMain = newUnit;
                    thisGatePatch.SetTransparent(GameManager.instance.GateTransparency);
                    //EconomyResources.Instance.Purchase(stats.resources.Gold);
                    Destroy(karta);
                    CardManager.instance.RevomeCard(karta);
                    UIController.Instance.ArrangeCards();
                    break;
                }

            }
            yield return null;


        }
        karta.GetComponent<Image>().color = CardManager.instance.defaultCardColor;
        _SpawnerCoroutine = null;
        yield return new WaitForSeconds(.1f);
        UIController.Instance.SwitchTrashcanActive();
        Debug.Log("wyczyszczono selected");
        _selectedCard = null;
        
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

    private bool CheckSpawnConditions(Gate gate, CardScriptableObject stats)
    {
        if (gate.path[0].unitMain != null)
        {
            EconomyConditions.Instance.ThereIsAUnit(); //Tu można zrobić funckje która na środku ekranu pokazuje tekst "HEX IS OCCUPIED"
            return false;
        }
        if (!Purchase(stats.resources))
        {
            EconomyConditions.Instance.NotEnoughCash(); //Tu można zrobić funckje która na środku ekranu pokazuje tekst "YOU CAN'T AFFORD IT / NOT ENOUGH FUNDS / NOT YOUR TURN"
            return false;
        }

        return true;
    }

    public void SetRemoved(bool value)
    {
        playerRemovedCard = value;
    }

    public void SpawnEnemyUnit(Gate gate, CardScriptableObject stats)
    {
        if (EnemyCheckSpawn(gate))
        {
            Vector3 rotation = gate.path.Last().position - gate.path[gate.path.Count - 2].position;
            Vector3 spawn = gate.path.Last().position;
            var unitStats = (UnitsStats)stats.GetStats();
            UnitControler newUnit = SpawnObjectAtLocation(spawn.x, spawn.y + 0.15f, spawn.z, rotation.y - 90f, unitStats.unit).GetComponent<UnitControler>();
            newUnit.SetSO(unitStats);
            newUnit.setMyGate(gate);
            gate.path.Last().unitMain = newUnit;
        }
    }

    public bool EnemyCheckSpawn(Gate gate)
    {
        return gate.path.Last().unitMain == null ? true : false;
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
