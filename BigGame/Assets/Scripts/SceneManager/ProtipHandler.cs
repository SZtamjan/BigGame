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

    private void OnEnable()
    {
        GetAndDisplayTip();
    }

    private void GetAndDisplayTip()
    {
        int index = Random.Range(0, textsSO.proTips.Count);
        if (!isGameplay)
        {
            tipTextDisplay.GetComponent<TextMeshProUGUI>().text = textsSO.proTips[index].ToString();
            PlayerPrefs.SetFloat("tipIndex", index);
        }
        else
        {
            Debug.Log("jestem w grze");
            tipTextDisplay.GetComponent<TextMeshProUGUI>().text = textsSO.proTips[Convert.ToInt32(PlayerPrefs.GetFloat("tipIndex"))].ToString();
        }
    }
}
