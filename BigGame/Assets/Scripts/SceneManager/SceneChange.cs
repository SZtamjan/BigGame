using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public partial class SceneChange : MonoBehaviour
{
    public GameObject loadingScreen;
    public Image loadingBarFill;
    public int nextScene;
    public GameObject loadingScreenNew;
    public TextMeshProUGUI lsText;
    
    public void LoadScene(int sceneId)
    {
        StartCoroutine(LoadSceneAsync(sceneId));
    }

    public void LoadMenu()
    {
        string idScena = "0";
        StartCoroutine(LoadSceneAsync(idScena));
    }
    
    public void LoadNextScene()
    {
        StartCoroutine(LoadSceneAsync());
    }
    IEnumerator LoadSceneAsync(string sceneId)
    {
        loadingScreen.SetActive(false);
        //yield return new WaitForSeconds(5f);

        int number = int.Parse(sceneId);
        
        AsyncOperation operation = SceneManager.LoadSceneAsync(number);

        while (!operation.isDone)
        {
            float progressValue = Mathf.Clamp01(operation.progress/0.9f);
            
            if(loadingBarFill != null) loadingBarFill.fillAmount = progressValue;

            yield return null;
        }
    }
    IEnumerator LoadSceneAsync(int sceneId)
    {
        loadingScreen.SetActive(true);
        yield return new WaitForSeconds(5f);

        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneId);

        while (!operation.isDone)
        {
            float progressValue = Mathf.Clamp01(operation.progress/0.9f);
            
            if(loadingBarFill != null) loadingBarFill.fillAmount = progressValue;

            yield return null;
        }
    }
    
    IEnumerator LoadSceneAsync()
    {
        loadingScreen.SetActive(true);
        Color col = loadingScreen.GetComponent<Image>().color;
        col.a = 1f;
        loadingScreen.GetComponent<Image>().color = col;

        Color tCol = lsText.color;
        tCol.a = 1f;
        lsText.color = tCol;
        
        yield return new WaitForSeconds(5f);

        nextScene = SceneManager.GetActiveScene().buildIndex+1;
        if (nextScene > 2)
        {
            nextScene = 0;
        }
        
        AsyncOperation operation = SceneManager.LoadSceneAsync(nextScene);

        while (!operation.isDone)
        {
            float progressValue = Mathf.Clamp01(operation.progress/0.9f);
            
            if(loadingBarFill != null) loadingBarFill.fillAmount = progressValue;

            yield return null;
        }
    }
    
}

public partial class SceneChange
{
    [SerializeField]public int sceneId;
    private bool useFixed = false;

    public void LoadOnClick()
    {
        StartCoroutine(LoadOnClickCor());
    }
    
    IEnumerator LoadOnClickCor()
    {
        bool clicked = false;
        while (clicked == false)
        {
            if (Input.anyKeyDown && useFixed == false)
            {
                LoadNextScene();
                useFixed = true;
                clicked = true;
            }
            yield return null;
        }

        yield return null;
    }
}