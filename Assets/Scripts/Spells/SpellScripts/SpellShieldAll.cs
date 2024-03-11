using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ScriptableObjects", menuName = "Spells/ShieldAll")]
public class SpellShieldAll : SpellsScrptableObject
{

 public void ShieldFrindlyUnits(bool playerUnit=true,int power=1)
    {
       
        foreach (var gate in CastlesController.Instance.playerCastle.gates)
        {
            foreach (var tile in gate.path)
            {
                var unit = tile.unitMain;
                if (unit == null)
                {
                    continue;
                }
                if (unit.IsThisPlayerUnit()!=playerUnit)
                {
                    continue;
                }
                unit.SpellShieldTaken(power);

            }
           
        }
    }
}
