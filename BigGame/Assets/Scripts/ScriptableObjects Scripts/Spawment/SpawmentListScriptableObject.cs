using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ScriptableObjects", menuName = "Spawment/List")]
public class SpawmentListScriptableObject : ScriptableObject
{
    [SerializeField]private List<SpawnTurnScriptableObject> ListForGates; 

   public UnitScriptableObjects SelectUnitAndTurnAndPath(int path, int turn)
    {
        if (ListForGates.Count==0)
        {
            return null;
        }

        return ListForGates[path].SelectUnitAndTurn(turn);

    }



}
