using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuickMenu : MonoBehaviour
{
    //Whole MENU
    public GameObject menu;

    public GameObject menuOne;
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
            menuOne.SetActive(true);
            //Reszte menu daæ false
        }
        else
        {
            menu.SetActive(true);
        }

    }


    public void Quit()
    {
        Application.Quit();
    }

}
