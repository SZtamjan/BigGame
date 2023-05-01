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
        BuildButton.SetActive(true);
    }

    public void ShowBuilding()
    {
        var buttons = BuildButton.GetComponent<CanvasGroup>();
        if (!build)
        {
            buttons.alpha = 1.0f;
            buttons.interactable = true;
            buttons.blocksRaycasts = true;
            build = true;
        }
        else
        {
            buttons.alpha = 0f;
            buttons.interactable = false;
            buttons.blocksRaycasts = false;
            build = false;
        }
        
    }

    

    
}
