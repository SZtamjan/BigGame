using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "ScriptableObjects", menuName = "Spells/OneTileFire")]
public class SpellOneTileFire : SpellsScrptableObject
{
    private bool _PlayerUnit;
    private int _Power;
    private GameObject _Karta;
    public override void SpellAction(GameObject karta, bool playerUnit = true, int power = 1)
    {

        karta.GetComponent<Image>().color = CardManager.instance.selectedCardColor;
        _PlayerUnit = playerUnit;
        _Power = power;
        _Karta = karta;
        SpellManager.Instance.SelectTile(DoSomethingWithThisUnit);
    }


    public void DoSomethingWithThisUnit(PathClass.Path path)
    {
        if (path == null|| path.unitMain==null)
        {
            _Karta.GetComponent<Image>().color = CardManager.instance.defaultCardColor;
        }
        else if (path.unitMain.IsThisPlayerUnit() == _PlayerUnit)
        {
            _Karta.GetComponent<Image>().color = CardManager.instance.defaultCardColor;
        }
        else if (path.unitMain.IsThisPlayerUnit() != _PlayerUnit)
        {
            path.unitMain.SpellDamageTaken(_Power);
            Destroy(_Karta);
        }

    }
    
}
