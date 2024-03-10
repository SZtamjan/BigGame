using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using static UnityEngine.GraphicsBuffer;

public class UnitCardMover : MonoBehaviour
{
    private RectTransform rectTransform;

    private Vector2 StartPos;
    private Coroutine _coroutine;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }
    private void Start()
    {
        StartPos = rectTransform.anchoredPosition;
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

    
    void OnMouseEnter()
    {
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
