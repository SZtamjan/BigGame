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
    public void SpawnUnit()
    {
        if (GameManager.instance.playerTurn && PatchControler.Instance.PlayerCastle.jednostka == null)
        {
            bool canIPurchase = Economy.Instance.Purchase(stats.cost);
            if (canIPurchase)
            {
                SpawnerScript.instance.SpawnMyUnit(stats);
            }
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
    public void MouseEnter()
    {
        isMovingUp = true;
        Vector2 targetPosition = StartPos + Vector2.up * 120f;
        StartCoroutine(MoveMe(targetPosition, true));
    }
    public void MouseExit()
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
