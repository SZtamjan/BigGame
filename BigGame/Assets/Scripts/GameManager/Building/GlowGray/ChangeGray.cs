using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeGray : MonoBehaviour
{
    private bool isOn = false;
    public bool xd = false;

    Renderer renderer;
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
        isOn = false;
    }

    public void ChangeBloom()
    {
        if (!Building.Instance.isBuilding)
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

                if (xd == true)
                {
                    Debug.Log("xd");
                    
                }
                mat.EnableKeyword("_EMISSION");
                
            }
            isOn = true;
        }
        
    }

    
}
