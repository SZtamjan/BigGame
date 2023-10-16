using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingsStats : MonoBehaviour
{
    [SerializeField] private int moneyGenerate = 0;
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
        unitAdd = stats.UnitAdd;
        if (unitAdd != null)
        {
            CardManager.instance.CardToDraw.Add(unitAdd);
            unitAdd = null;
        }
    }

    public int returnMoneyGain()
    {
        // tu w ramach potrzeb mo�na dopisa� jakie� warunki na generowanie pienia�k�w

        return moneyGenerate;

    }

    void BuildingDoingSomething()
    {
        Economy.Instance.cash += returnMoneyGain();

    }




}
