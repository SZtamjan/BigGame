using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ScriptableObjects", menuName = "Spawment/Turn")]
public class SpawnTurnScriptableObject : ScriptableObject
{
    [Tooltip("numer tury")]
    [SerializeField] private List<SpawnUnitsScriptableObject> ListForUnits;

    public UnitScriptableObjects SelectUnitAndTurn(int turn)
    {
        if (ListForUnits == null)
        {
            return null;
        }
        if (turn > ListForUnits.Count)
        {
            while (turn > ListForUnits.Count)
            {
                turn -= ListForUnits.Count;
            }
        }
        return ListForUnits[turn-1].SelectUnit();
    }
}
