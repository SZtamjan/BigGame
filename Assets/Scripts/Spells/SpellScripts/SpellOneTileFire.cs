using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


[CreateAssetMenu(fileName = "ScriptableObjects", menuName = "Spells/OneTileFire")]
public class SpellOneTileFire : SpellsScrptableObject
{
    private bool _PlayerUnit;
    private int _Power;
    public void SelectTileToFire(bool playerUnit = true, int power = 1)
    {
       
        _PlayerUnit = playerUnit;
        _Power = power;
        SpellManager.Instance.SelectTile(DoSomethingWithThisUnit);
    }


    public void DoSomethingWithThisUnit(PathClass.Path path)
    {
        if (path == null|| path.unitMain==null)
        {
            
        }
        else if (path.unitMain.IsThisPlayerUnit() == _PlayerUnit)
        {

        }
        else if (path.unitMain.IsThisPlayerUnit() != _PlayerUnit)
        {
            path.unitMain.SpellDamageTaken(_Power);
        }


    }


}
