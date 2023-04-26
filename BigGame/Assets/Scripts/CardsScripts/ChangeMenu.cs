using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeMenu : MonoBehaviour
{
    [SerializeField] private GameObject UnitsButton;
    [SerializeField] private GameObject BuildButton;
    private bool build = false;
    public static ChangeMenu instance;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        UnitsButton.SetActive(true);
        BuildButton.SetActive(false);
    }

    public void ShowBuilding()
    {
        if (!build)
        {
            BuildButton.SetActive(true);
            build = true;
        }
        else
        {
            BuildButton.SetActive(false);
            build = false;
        }
        
    }

    

    
}
