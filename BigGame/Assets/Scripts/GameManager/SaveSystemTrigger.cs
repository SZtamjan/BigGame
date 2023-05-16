using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveSystemTrigger : MonoBehaviour
{
    public int level;
    //kolejne zmienne
    
    public void SaveLevel()
    {
        level = SceneManager.GetActiveScene().buildIndex;
        //kolejne zmienne
        
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
