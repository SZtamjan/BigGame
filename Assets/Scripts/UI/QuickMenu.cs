using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuickMenu : MonoBehaviour
{
    //Whole MENU
    public GameObject menu;
    public GameObject menuBacker;
    
    public List<GameObject> menus; 
    //public GameObject menuTwo;
    //public GameObject menuThree;

    void Start()
    {
        menu.SetActive(false);
    }

    public void GoToMenu()
    {
        menuBacker.SetActive(true);
        menuBacker.GetComponent<SceneChange>().LoadMenu();
    }
    
    public void ChangeVis()
    {
        if (menu.gameObject.activeSelf)
        {
            menu.SetActive(false);
            menus[0].SetActive(true);
            menus[1].SetActive(false);
            menus[2].SetActive(false);
        }
        else
        {
            menu.SetActive(true);
        }

    }

    public void HideSettings()
    {
        menus[0].SetActive(true);
        menus[1].SetActive(false);
    }

    public void ExitConfirm()
    {
        menus[2].SetActive(true);
    }
    public void ExitExitConfirm()
    {
        menus[2].SetActive(false);
    }

    public void ShowSettings()
    {
        menus[0].SetActive(false);
        menus[1].SetActive(true);
    }
}
