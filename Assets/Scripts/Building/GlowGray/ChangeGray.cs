using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SelectionBase]
public class ChangeGray : MonoBehaviour
{
    private new Renderer renderer;
    Material[] materials;

    private void Start()
    {
        renderer = GetComponent<Renderer>();
        materials = renderer.materials;
        
        foreach (Material mat in materials)
        {
            mat.SetColor("_EmissionColor",Building.Instance.notPlaceableColor);
            mat.DisableKeyword("_EMISSION");
        }
    }

    public void ChangeBloom()
    {
        if (!Building.Instance.isBuilding)
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
        }
        else
        {
            foreach (Material mat in materials)
            {
                if (Building.Instance.isColor)
                {
                    mat.SetColor("_BaseColor",Building.Instance.regularNotPlaceableColor);
                }
                else
                {
                    mat.EnableKeyword("_EMISSION");
                }
            }
        }
        
    }

    
}
