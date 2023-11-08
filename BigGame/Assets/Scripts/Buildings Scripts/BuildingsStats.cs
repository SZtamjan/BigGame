using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingsStats : MonoBehaviour
{
    [SerializeField] private int moneyGenerate = 0;
    public WhichBudynek thisBudynekIs;
    [SerializeField] private UnitScriptableObjects unitAdd;

    private void OnEnable()
    {
        EventManager.BuildingAction += BuildingDoingSomething;
    }

    private void OnDisable()
    {
        EventManager.BuildingAction -= BuildingDoingSomething;
    }


    public void putStats(BuildingsScriptableObjects stats)
    {
        moneyGenerate = stats.moneyGain;
        thisBudynekIs = stats.whichBudynek;
        unitAdd = stats.UnitAdd;
        if (unitAdd != null)
        {
            CardManager.instance.CollectionCardsToDraw.Add(unitAdd);
            unitAdd = null;
        }
    }

    public int returnMoneyGain()
    {
        // tu w ramach potrzeb można dopisać jakieś warunki na generowanie pieniażków

        return moneyGenerate;

    }

    void BuildingDoingSomething()
    {
        Economy.Instance.cash += returnMoneyGain();

    }




}