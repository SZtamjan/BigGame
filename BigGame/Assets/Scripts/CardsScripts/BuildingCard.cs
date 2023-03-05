using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildingCard : MonoBehaviour
{
    public GameObject stucture;
    public void StartBulding()
    {

        GameManager.gameManager.GetComponent<Building>().StartBuilding(stucture, gameObject);
    }
}
