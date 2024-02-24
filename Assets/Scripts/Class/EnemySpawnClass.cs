using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnClass : MonoBehaviour
{
    [System.Serializable]
    public class EnemySpawn
    {
        public UnitScriptableObjects unitToSpawn;
        public int turnCount=3;
    }
}
