using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class RotateHex : MonoBehaviour
{
    private void Start()
    {
        RotateThisHex();
    }

    public void RotateThisHex()
    {
        Quaternion newRotation = Quaternion.Euler(0,0,0);
        
        float range = Random.Range(0, 6);
        
        for (int i = 0; i < range; i++)
        {
            newRotation *= Quaternion.Euler(0,60,0);
        }
        
        gameObject.transform.rotation = newRotation;
    }
    
}
