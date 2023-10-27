using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingsStats : MonoBehaviour
{
    [SerializeField] private int moneyGenerate = 0;
    [SerializeField] private WhichBudynek thisBudynekIs;

    public void putStats(BuildingsScriptableObjects stats)
    {
        moneyGenerate = stats.moneyGain;
        thisBudynekIs = stats.whichBudynek;
    }

    public int returnMoneyGain()
    {
        // tu w ramach potrzeb można dopisać jakieś warunki na generowanie pieniażków

        return moneyGenerate;

    }


}