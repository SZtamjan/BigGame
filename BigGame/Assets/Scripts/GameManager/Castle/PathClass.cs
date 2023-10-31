using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathClass : MonoBehaviour
{
    [System.Serializable]
    public class Path
    {
        public Vector3 position;
        public GameObject unitMain;
        public GameObject unitWanting;
    }
}
