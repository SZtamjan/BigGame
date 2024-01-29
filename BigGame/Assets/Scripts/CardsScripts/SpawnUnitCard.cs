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
    private Coroutine _coroutine;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }
    private void Start()
    {
        GetCardStats();

        StartPos = rectTransform.anchoredPosition;

    }
    public void InitBuy()
    {
        SpawnerScript.instance.SpawnMyUnit(gameObject, stats);
    }

    public void NewStartPos()
    {
        StartPos = rectTransform.anchoredPosition;
        if (_coroutine != null)
        {
            StopCoroutine(_coroutine);
        }
        _coroutine = StartCoroutine(MoveMe(StartPos, false));
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
        if (_coroutine != null)
        {
            StopCoroutine(_coroutine);
        }
        _coroutine = StartCoroutine(MoveMe(targetPosition, true));
        transform.SetAsLastSibling();

    }
    void OnMouseExit()
    {
        isMovingUp = false;
        Vector2 targetPosition = StartPos;
        if (_coroutine != null)
        {
            StopCoroutine(_coroutine);
        }
        _coroutine = StartCoroutine(MoveMe(targetPosition, false));
    }



    private IEnumerator MoveMe(Vector2 targetPosition, bool up)
    {
        while (true)
        {

            rectTransform.anchoredPosition = Vector2.Lerp(rectTransform.anchoredPosition, targetPosition, Time.deltaTime * 5);
            if (Vector2.Distance(rectTransform.anchoredPosition, targetPosition) == 0f)
            {
                _coroutine = null;
                break;
            }
            yield return null;
        }
    }



}
