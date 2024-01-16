using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ScriptableObjects", menuName = "Spawment/Turn")]
public class SpawnTurnScriptableObject : ScriptableObject
{
    [Tooltip("Co GATE bêdzie spawnowaæ w danej turze")]
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
        if (ListForUnits[turn - 1] == null)
        {
            return null;
        }
        return ListForUnits[turn-1].SelectUnit();
    }
}
