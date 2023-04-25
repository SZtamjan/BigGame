using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ProtipHandler : MonoBehaviour
{
    public ProtipText textsSO;
    public GameObject tipTextDisplay;

    private void Start()
    {
        int index = Random.Range(0, textsSO.proTips.Count);
        tipTextDisplay.GetComponent<TextMeshProUGUI>().text = textsSO.proTips[index].ToString();
    }
}
