using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SceneHandler : MonoBehaviour
{
    [Header("Screens")]
    [SerializeField] private GameObject handler;
    [SerializeField] private TextMeshProUGUI txt;
    [SerializeField] private GameObject press;

    private bool onlyOne = false;

    void Update()
    {
        if (Input.anyKeyDown) AnyButton();
    }

    public void AnyButton()
    {
        if (onlyOne == false)
        {
            PlaySonk(); //Totally by accident
            txt.text = "Wygraj aby wygraæ!";
            StartCoroutine(LoadingScreenFadeOut()); //Loading screen fadeout
            press.SetActive(false);
        }
        onlyOne = true;
    }

    private void PlaySonk()
    {
        AudioManager.instance.GetComponent<AudioManager>().KorutynaCzas();
    }


    IEnumerator LoadingScreenFadeOut()
    {
        Color imageColor = handler.GetComponent<Image>().color;
        Color textColor = handler.GetComponentInChildren<TextMeshProUGUI>().color;

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

            handler.GetComponentInChildren<TextMeshProUGUI>().color = textColor;
            handler.GetComponent<Image>().color = imageColor;
            yield return null;
            elapsedTime += Time.deltaTime;
        }

        if (imageColor.a == 0) Destroy(handler);
    }
}