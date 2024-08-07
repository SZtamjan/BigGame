using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EconomyConditions : MonoBehaviour
{
    public static EconomyConditions Instance;

    [Header("Warning Text Settings")]
    public float textFullAlpha = 2f;
    public float fadeDuration = 2f;



    private void Awake()
    {
        Instance = this;
    }

    public bool CheckIfHexEmpty()
    {
        if (PathControler.Instance.PlayerCastle.jednostka == null)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void NotEnoughCash()
    {
        UIController.Instance.WarmingShowWarming("Not Enough Cash!1!11!");
        Debug.Log("Not enough cash");
    }

    public void NotEnoughResources()
    {
        UIController.Instance.WarmingShowWarming("Not Enough Resources!1!11!");
        Debug.Log("Not enough resources");
    }

    public void ThereIsABuilding()
    {
        UIController.Instance.WarmingShowWarming("Obiekt tu jest!1!11!");
        Debug.Log("Obiekt tu jest");
    }

    public void HereIsNotAPlaceToBuild()
    {
        Debug.Log("Tu nie mo�na budowa�");
        UIController.Instance.WarmingShowWarming("Tu nie mo�na tego budowa�");
    }

    public void ThereIsAUnit()
    {
        UIController.Instance.WarmingShowWarming("Jednostka tu jest!1!11!");
        Debug.Log("Jednostka tu jest");
    }

    public void BuildingLimitAchieved()
    {
        UIController.Instance.WarmingShowWarming("Limit of buildings achieved!");
        Debug.Log("Limit of buildings achieved!");
    }

    public void NotUrTurn()
    {
        UIController.Instance.WarmingShowWarming("Wait for your turn!");
        Debug.Log("Not players turn");
    }

}
