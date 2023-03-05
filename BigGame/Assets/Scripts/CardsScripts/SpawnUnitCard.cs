using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnUnitCard : MonoBehaviour
{

    public GameObject Unit;

    public void SpawnUnit()
    {

        GameManager.gameManager.GetComponent<SpawnerScript>().SpawnMyUnit(Unit);
    }
}
