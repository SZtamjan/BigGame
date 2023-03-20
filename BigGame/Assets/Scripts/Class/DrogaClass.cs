using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrogaClass : MonoBehaviour
{

    [System.Serializable]
    public class Droga
    {
        public Vector3 coordinations;
        public GameObject unit;
        public GameObject wantingUnit;
        public int holdPower;
    }
}