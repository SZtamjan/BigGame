using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EconomyConditions : MonoBehaviour
{
    public static EconomyConditions Instance;

    public TextMeshProUGUI warning;

    [Header("Warning Text Settings")]
    public float textFullAlpha = 2f;
    public float fadeDuration = 2f;
    private float currentAlpha;
    private float targetAlpha = 0f;


    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        warning.alpha = 0f;
    }

    public bool CheckIfHexEmpty()
    {
        if (PathControler.Instance.PlayerCastle.jednostka == null)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void NotEnoughCash()
    {
        warning.text = "Not Enough Cash!1!11!";
        StartCoroutine(WarningLenght());
        Debug.Log("Not enough cash");
    }

    public void ThereIsABuilding()
    {
        warning.text = "Obiekt tu jest!1!11!";
        StartCoroutine(WarningLenght());
        Debug.Log("Obiekt tu jest");
    }

    public void ThereIsAUnit()
    {
        warning.text = "Jednostka tu jest!1!11!";
        StartCoroutine(WarningLenght());
        Debug.Log("Jednostka tu jest");
    }

    IEnumerator WarningLenght()
    {
        float elapsedTime = 0f;
        warning.alpha = 1;
        currentAlpha = warning.alpha;

        yield return new WaitForSeconds(textFullAlpha);

        while(elapsedTime<fadeDuration)
        {
            float newAlpha = Mathf.Lerp(currentAlpha, targetAlpha, elapsedTime / fadeDuration);

            warning.alpha = newAlpha;
            yield return null;

            elapsedTime += Time.deltaTime;
        }
    }
}
