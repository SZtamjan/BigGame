using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeGlow : MonoBehaviour
{
    private bool isOn = false;
    private new Renderer renderer;
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
                if (Building.Instance.isColor)
                {
                    mat.SetColor("_BaseColor",Color.white);
                }
                else
                {
                    mat.DisableKeyword("_EMISSION");
                }
                
            }
            isOn = false;
        }
        else
        {
            foreach (Material mat in materials)
            {
                if (Building.Instance.isColor)
                {
                    mat.SetColor("_BaseColor",Building.Instance.regularPlaceableColor);
                }
                else
                {
                    mat.EnableKeyword("_EMISSION");
                }
            }
            isOn = true;
        }
        
    }
}
