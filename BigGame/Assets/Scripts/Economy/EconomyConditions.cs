using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EconomyConditions : MonoBehaviour
{
    public static EconomyConditions Instance;

    public TextMeshProUGUI warning;

    [Header("Warning Text Settings")]
    public float textFullAlpha = 2f;
    public float fadeDuration = 2f;
    


    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        warning.alpha = 0f;
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

    public void ThereIsABuilding()
    {
        UIController.Instance.WarmingShowWarming("Obiekt tu jest!1!11!");
        Debug.Log("Obiekt tu jest");
    }

    public void ThereIsAUnit()
    {
        UIController.Instance.WarmingShowWarming("Jednostka tu jest!1!11!");
        Debug.Log("Jednostka tu jest");
    }

}
