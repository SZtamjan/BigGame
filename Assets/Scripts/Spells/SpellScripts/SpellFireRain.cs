using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "ScriptableObjects", menuName = "Spells/FireRain")]
public class SpellFireRain : SpellsScrptableObject
{
    public override void SpellAction(GameObject karta,bool playerUnit = true, int power = 1)
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
                if (unit.IsThisPlayerUnit() == playerUnit)
                {
                    continue;
                }
                unit.SpellDamageTaken(power);

            }

        }
    }

    
}
