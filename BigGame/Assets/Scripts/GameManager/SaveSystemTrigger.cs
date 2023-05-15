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
        Debug.Log("SaveSystemTrigger: " + level);
        SaveSystem.SaveLevel(this);
    }

    public int LoadLevel()
    {
        DataToSave data = SaveSystem.LoadLevel();

        level = data.level;
        //kolejne zmienne
        
        return level;
    }

}
