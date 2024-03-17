using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "ScriptableObjects", menuName = "Spells/Main")]
public abstract class SpellsScrptableObject : ScriptableObject
{
    public abstract void SpellAction(GameObject karta, bool playerUnit = true, int power = 1);

}
