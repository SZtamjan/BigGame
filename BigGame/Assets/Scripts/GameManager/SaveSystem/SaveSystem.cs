using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class SaveSystem : MonoBehaviour
{
    private static string levelPath = Application.persistentDataPath + "/level.xd";
    private static string tipPath = Application.persistentDataPath + "/tip.pog";
    public GameData gameData;

    public static void SaveLevel(SaveSystemTrigger levelSave)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = new FileStream(levelPath, FileMode.Create);
        Debug.Log("SaveSystem: " + levelSave.level);
        DataToSave data = new DataToSave(levelSave);
        
        formatter.Serialize(stream,data);
        stream.Close();
    }

    public static DataToSave LoadLevel()
    {
        if (File.Exists(levelPath))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(levelPath, FileMode.Open);
            
            DataToSave data = formatter.Deserialize(stream) as DataToSave;

            stream.Close();
            return data;
        }
        else
        {
            Debug.LogError("Save file not found in " + levelPath);
            return null;
        }
    }
}
