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

    private void Start()
    {
        GetAndDisplayTip();
    }

    private void GetAndDisplayTip()
    {
        if (!isGameplay)
        {
            tipNO = Random.Range(0, textsSO.proTips.Count);
            PlayerPrefs.SetFloat("tipIndex", tipNO);
            tipTextDisplay.GetComponent<TextMeshProUGUI>().text = textsSO.proTips[tipNO].ToString();
        }
        else
        {
            Debug.Log("jestem w grze");
            int a = (int)PlayerPrefs.GetFloat("tipIndex");
            tipTextDisplay.GetComponent<TextMeshProUGUI>().text = textsSO.proTips[a].ToString();
        }
    }
}
