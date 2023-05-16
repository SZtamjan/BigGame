using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class ProtipHandler : MonoBehaviour
{
    public ProtipText textsSO;
    public GameObject tipTextDisplay;
    public bool isGameplay;
    private int tipNO;

    private void OnEnable()
    {
        GetAndDisplayTip();
    }

    private void GetAndDisplayTip()
    {
        if (!isGameplay)
        {
            tipNO = Random.Range(0, textsSO.proTips.Count);
            MenuManager.instance.gameObject.GetComponent<SaveSystemTrigger>().SaveTipNO(tipNO);
            tipTextDisplay.GetComponent<TextMeshProUGUI>().text = textsSO.proTips[tipNO].ToString();
            
            PlayerPrefs.SetFloat("tipIndex", tipNO);
        }
        else
        {
            Debug.Log("jestem w grze");
            int a = SaveSystemTrigger.instance.LoadTip();
            tipTextDisplay.GetComponent<TextMeshProUGUI>().text = textsSO.proTips[a].ToString();
        }
    }
}
