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

    public void ChangeMenuCards()
    {
        if (build)
        {
            UnitsButton.SetActive(true);
            BuildButton.SetActive(false);
            build = false;
        }
        else
        {
            UnitsButton.SetActive(false);
            BuildButton.SetActive(true);
            build = true;
        }
    }

    public void Hide () 
    {
        UnitsButton.SetActive(false);
        BuildButton.SetActive(false);
    }

    public void UnHide()
    {
        UnitsButton.SetActive(false);
        BuildButton.SetActive(true);
    }
}
