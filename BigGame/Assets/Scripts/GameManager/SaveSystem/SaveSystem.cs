using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class SaveSystem : MonoBehaviour
{
    public GameData gameData;

    public void SaveLevel()
    {
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = new FileStream(SavePaths.levelPath, FileMode.Create);
        Debug.Log("Level "+ gameData.sceneIndex + " Saved" + SavePaths.levelPath);

        formatter.Serialize(stream,gameData);
        stream.Close();
    }

    public GameData LoadLevel()
    {
        if (File.Exists(SavePaths.levelPath))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(SavePaths.levelPath, FileMode.Open);
            
            GameData data = (GameData)formatter.Deserialize(stream);

            stream.Close();
            return data;
        }
        else
        {
            Debug.LogError("Save file not found in " + SavePaths.levelPath);
            return new GameData();
        }
    }
}
