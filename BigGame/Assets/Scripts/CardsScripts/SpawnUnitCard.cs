using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;

public class SpawnUnitCard : MonoBehaviour
{
    public UnitScriptableObjects stats;

    public new TextMeshProUGUI name;
    public TextMeshProUGUI desc;
    public TextMeshProUGUI cost;

    private bool isMovingUp;
    private RectTransform rectTransform;

    private Vector2 StartPos;


    private void Start()
    {
        GetCardStats();
        rectTransform = GetComponent<RectTransform>();
        StartPos = rectTransform.anchoredPosition;

    }
    public void InitBuy()
    {
        bool hexIsEmpty = EconomyConditions.Instance.CheckIfHexEmpty();
        if (hexIsEmpty)
        {
            bool CanIBuy = Economy.Instance.CanIBuy(stats.cost);
            if (CanIBuy)
            {
                Economy.Instance.Purchase(stats.cost);
                GameManager.gameManager.GetComponent<SpawnerScript>().SpawnMyUnit(stats);
            }
            else
            {
                EconomyConditions.Instance.NotEnoughCash(); //Tu można zrobić funckje która na środku ekranu pokazuje tekst "YOU CAN'T AFFORD IT / NOT ENOUGH FUNDS"
            }
        }
        else
        {
            EconomyConditions.Instance.ThereIsAUnit(); //Tu można zrobić funckje która na środku ekranu pokazuje tekst "HEX IS OCCUPIED"
        }
    }

    public void GetCardStats()
    {
        Sprite cardArtwork = stats.artwork;

        gameObject.GetComponent<Image>().sprite = cardArtwork;
        name.text = stats.name;
        desc.text = stats.desc;
        cost.text = stats.cost.ToString();
    }
    void OnMouseEnter()
    {
        isMovingUp = true;
        Vector2 targetPosition = StartPos + Vector2.up * 120f;
        StartCoroutine(MoveMe(targetPosition, true));
    }
    void OnMouseExit()
    {
        isMovingUp = false;
        Vector2 targetPosition = StartPos;
        StartCoroutine(MoveMe(targetPosition, false));
    }



    private IEnumerator MoveMe(Vector2 targetPosition, bool up)
    {
        while (up == isMovingUp)
        {

            rectTransform.anchoredPosition = Vector2.Lerp(rectTransform.anchoredPosition, targetPosition, Time.deltaTime * 5);
            if (Vector2.Distance(rectTransform.anchoredPosition, targetPosition) == 0f)
            {
                break;
            }
            yield return null;
        }
    }



}
