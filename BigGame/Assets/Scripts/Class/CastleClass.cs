using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CastleClass : MonoBehaviour
{
    [System.Serializable]
    public class Castle
    {
        public GameObject castle;
        public GameObject jednostka = null;
    }

    [System.Serializable]
    public class CastleArmaments
    {
        int damage;
        int reach;
    }

}
