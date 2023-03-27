using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Prices : MonoBehaviour
{
    public int podatek = 23;
    public int budynek = 10;

    [SerializeField]
    public static Prices prices;

    private void Start()
    {
        prices = GetComponent<Prices>();
    }

}
