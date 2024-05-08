using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridInfoLoader : MonoBehaviour
{
    [SerializeField] private GameObject BuildingsHolder;

    private void Start()
    {
        if (BuildingsHolder == null)
        {
            Debug.LogWarning("UZUPELNIJ BUILDGIN HOLDER");
            return;
        }

        Building.Instance.parent = BuildingsHolder;
    }
}
