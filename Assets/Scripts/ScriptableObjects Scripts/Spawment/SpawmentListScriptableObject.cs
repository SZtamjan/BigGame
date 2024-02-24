using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ScriptableObjects", menuName = "Spawment/List")]
public class SpawmentListScriptableObject : ScriptableObject
{
    [SerializeField][Tooltip("Lista przypisania co GATE ma spawnować")]private List<SpawnTurnScriptableObject> ListForGates; 

   public UnitScriptableObjects SelectUnitAndTurnAndPath(int path, int turn)
    {
        if (ListForGates.Count==0)
        {
            return null;
        }
        if (ListForGates[path]==null)
        {
            Debug.Log("Gate nie ma co spawnować, nie przypisane poprawnie w Spawment/List");
            return null;            
        }
        return ListForGates[path].SelectUnitAndTurn(turn);

    }



}
