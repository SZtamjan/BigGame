using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitsStatsClass
{
    [System.Serializable]
    public class UnitsStats
    {
        public GameObject unit;
        public int hp;
        public int damage;
        public int movmentDistance;
        public int attackReach;
        public bool playersSide;
        public float moveSpeed;
    }

}
