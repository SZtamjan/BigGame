using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SceneHandler : MonoBehaviour
{
    [Header("Screens")]
    [SerializeField] private GameObject handler;
    [SerializeField] private GameObject loadingScreenOne;
    [SerializeField] private GameObject loadingScreenTwo;

    private int counter = 0;

    void Update()
    {
        if (Input.anyKeyDown) AnyButton();
    }

    public void AnyButton()
    {
        if (counter<1)
        {
            ChangeScreen();
            PlaySonk(); //Totally by accident
            StartCoroutine(LoadingScreenFadeOut()); //Loading screen fadeout
        }
        counter++;
    }

    private void PlaySonk()
    {
        AudioManager.instance.GetComponent<AudioManager>().KorutynaCzas();
    }

    private void ChangeScreen()
    {
        loadingScreenOne.SetActive(!true);
        loadingScreenTwo.SetActive(true);
    }

    IEnumerator LoadingScreenFadeOut()
    {
        Color imageColor = loadingScreenTwo.GetComponent<Image>().color;
        Color textColor = loadingScreenTwo.GetComponentInChildren<TextMeshProUGUI>().color;

        float startAlpha = imageColor.a;
        float targetAlpha = 0f;

        float fadeDuration = 4f;
        float elapsedTime = 0f;

        yield return new WaitForSeconds(1f);

        while (imageColor.a != 0)
        {
            float newAlpha = Mathf.Lerp(startAlpha, targetAlpha, elapsedTime / fadeDuration);
            imageColor.a = newAlpha;
            textColor.a = newAlpha;

            loadingScreenTwo.GetComponentInChildren<TextMeshProUGUI>().color = textColor;
            loadingScreenTwo.GetComponent<Image>().color = imageColor;
            yield return null;
            elapsedTime += Time.deltaTime;
        }

        if (imageColor.a == 0) Destroy(handler);
    }
}
