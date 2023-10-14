using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static class SaveSystem
{
    private static string levelPath = Application.persistentDataPath + "/level.xd";
    private static string tipPath = Application.persistentDataPath + "/tip.pog";
    
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
    
    public static void SaveTipNOSS(SaveSystemTrigger tipSave)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = new FileStream(tipPath, FileMode.Create);
        Debug.Log("tip number: " + tipSave.tipNo);
        tipData data = new tipData(tipSave);
        
        formatter.Serialize(stream,data);
        stream.Close();
    }
    public static tipData LoadTip()
    {
        if (File.Exists(tipPath))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(tipPath, FileMode.Open);
            
            tipData data = formatter.Deserialize(stream) as tipData;

            stream.Close();
            return data;
        }
        else
        {
            Debug.LogError("Save file not found in " + tipPath);
            return null; 
        }
    }
    
}
