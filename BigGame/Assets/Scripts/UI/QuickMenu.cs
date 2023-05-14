using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuickMenu : MonoBehaviour
{
    //Whole MENU
    public GameObject menu;

    public List<GameObject> menus; 
    //public GameObject menuTwo;
    //public GameObject menuThree;

    void Start()
    {
        menu.SetActive(false);
    }

    public void ChangeVis()
    {
        if (menu.gameObject.activeSelf)
        {
            menu.SetActive(false);
            menus[0].SetActive(true);
            //Reszte menu da� false
        }
        else
        {
            menu.SetActive(true);
        }

    }

    public void ExitConfirm()
    {
        menus[1].SetActive(true);
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void ExitNo()
    {
        menus[1].SetActive(false);
    }

}
