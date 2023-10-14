using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeGlow : MonoBehaviour
{
    private bool isOn = false;

    private void Start()
    {
        gameObject.GetComponent<MeshRenderer>().material.DisableKeyword("_EMISSION");
        isOn = false;
    }

    public void ChangeBloom()
    {
        if (isOn)
        {
            gameObject.GetComponent<MeshRenderer>().material.DisableKeyword("_EMISSION");
            isOn = false;
        }
        else
        {
            gameObject.GetComponent<MeshRenderer>().material.EnableKeyword("_EMISSION");
            isOn = true;
        }
        
    }
}
