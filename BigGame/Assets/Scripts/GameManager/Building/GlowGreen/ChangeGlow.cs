using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeGlow : MonoBehaviour
{
    private bool isOn = false;
    Renderer renderer;
    Material[] materials;

    private void Start()
    {
        renderer = GetComponent<Renderer>();
        materials = renderer.materials;
        
        foreach (Material mat in materials)
        {
            mat.SetColor("_EmissionColor",Building.Instance.placeableColor);
            mat.DisableKeyword("_EMISSION");
        }
        isOn = false;
    }

    public void ChangeBloom()
    {
        if (isOn)
        {
            foreach (Material mat in materials)
            {
                mat.DisableKeyword("_EMISSION");
            }
            isOn = false;
        }
        else
        {
            foreach (Material mat in materials)
            {
                mat.EnableKeyword("_EMISSION");
            }
            isOn = true;
        }
        
    }
}
