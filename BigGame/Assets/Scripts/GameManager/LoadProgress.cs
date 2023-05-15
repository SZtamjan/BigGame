using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadProgress : MonoBehaviour
{
    private int levelToLoad;

    public void LoadLevel()
    {
        levelToLoad = GetComponent<SaveSystemTrigger>().LoadLevel();
        GetComponent<SceneChange>().LoadScene(levelToLoad);
    }
}
