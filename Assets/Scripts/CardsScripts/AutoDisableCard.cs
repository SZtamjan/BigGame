using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Economy.EconomyActions;
using TMPro;
using UnityEngine.UI;

public class AutoDisableCard : EconomyOperations
{
    private void Start()
    {
        //Powinno git dzialac, tylko trzeba poustawiac w calej grze odpowiednio
        //zeby w ResourcesStruct przechodzil update wartosci przez setter
        EconomyResources.Instance.Resources.updatedResources.AddListener(ChangeGray);
        ChangeGray();
    }

    private void ChangeGray()
    {
        if (!CheckIfICanIAfford(GetComponent<UnitCardStats>().Stats.resources))
        {
            GetComponent<Button>().interactable = false;

            TextMeshProUGUI[] texts = GetComponent<UnitCardStats>().ReturnTexts();

            Color color = GetComponent<Button>().colors.disabledColor;
            
            foreach (var text in texts)
            {
                text.color = color;
            }
        }
        else
        {
            GetComponent<Button>().interactable = true;

            TextMeshProUGUI[] texts = GetComponent<UnitCardStats>().ReturnTexts();

            Color color = GetComponent<Button>().colors.normalColor;
            
            foreach (var text in texts)
            {
                text.color = color;
            }
        }
    }
}
