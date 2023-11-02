using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveSystemTrigger : MonoBehaviour
{
    public static SaveSystemTrigger instance;
    
    public int level;
    public int tipNo;
    //kolejne zmienne

    private void Awake()
    {
        instance = this;
    }

    public void SaveLevel()
    {
        level = SceneManager.GetActiveScene().buildIndex;

        SaveSystem.SaveLevel(this);
    }
    
    public int LoadLevel()
    {
        DataToSave data = SaveSystem.LoadLevel();

        level = data.level;
        //kolejne zmienne
        Debug.Log("Load: " + level);
        
        return level;
    }
}
