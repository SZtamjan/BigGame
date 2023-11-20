using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadSavedLevelScript : MonoBehaviour
{
    private int levelToLoad;

    public void LoadLevel()
    {
        levelToLoad = GetComponent<SaveSystem>().LoadLevel().sceneIndex;
        GetComponent<SceneChange>().LoadScene(levelToLoad);
    }
}
