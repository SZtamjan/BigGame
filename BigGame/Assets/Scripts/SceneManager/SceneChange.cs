using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public partial class SceneChange : MonoBehaviour
{
    public GameObject loadingScreen;
    public Image loadingBarFill;

    public void LoadScene(int sceneId)
    {
        StartCoroutine(LoadSceneAsync(sceneId));
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
                GetComponent<SceneChange>().LoadScene(sceneId);
                useFixed = true;
                clicked = true;
            }
            yield return null;
        }

        yield return null;
    }
}