using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static class SaveSystem
{
    private static string path = Application.persistentDataPath + "/level.xd";
    public static void SaveLevel(SaveSystemTrigger levelSave)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = new FileStream(path, FileMode.Create);
        Debug.Log("SaveSystem: " + levelSave.level);
        DataToSave data = new DataToSave(levelSave);
        
        formatter.Serialize(stream,data);
        stream.Close();
    }
    
    public static DataToSave LoadLevel()
    {
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);
            
            DataToSave data = formatter.Deserialize(stream) as DataToSave;

            stream.Close();
            return data;
        }
        else
        {
            Debug.LogError("Save file not found in " + path);
            return null;
        }
    }
}
