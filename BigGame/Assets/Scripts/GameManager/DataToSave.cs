using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class DataToSave
{
    public int level;
    //kolejne zmienne
    public DataToSave(SaveSystemTrigger lvlMethod)
    {
        level = lvlMethod.level;
        //kolejne zmienne
        
        Debug.Log("DataToSave: " + level);
    }
}
