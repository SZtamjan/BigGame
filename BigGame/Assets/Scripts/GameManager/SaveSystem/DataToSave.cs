using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class DataToSave
{
    public int level;
    
    public DataToSave(SaveSystemTrigger lvlMethod)
    {
        level = lvlMethod.level;
    }
}