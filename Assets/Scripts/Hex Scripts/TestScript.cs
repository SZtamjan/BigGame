using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SelectionBase]
public class TestScript : MonoBehaviour
{
    public Material material;
    private Coroutine _coroutine = null;
    public float time = 2;


    [Range(0.0f, 1.0f)]
    public float ditter = 1;
    public Material myMat;

   


    // Update is called once per frame
    void Update()
    {
        material.SetFloat("_DitherThreshold", ditter);
    }
    [Button]
    private void D00()
    {
        ChangeDitter(0f);
    }

    [Button]
    private void D10()
    {
        ChangeDitter(0.1f);
    }

    [Button]
    private void D50()
    {
        ChangeDitter(0.5f);
    }

    [Button]
    private void D100()
    {
        ChangeDitter(1f);
    }
    private void ChangeDitter(float target)
    {
        if (_coroutine != null)
        {
            StopCoroutine(_coroutine);
        }
        _coroutine = StartCoroutine(changedithering(target));
    }

    IEnumerator changedithering(float target)
    {
        float timeRemaining = 0f;
        var tempDitter = ditter;
        while (timeRemaining < time)
        {           
            ditter = Mathf.Lerp(tempDitter, target, (timeRemaining / time));
            timeRemaining += Time.deltaTime;
            yield return null;
        }
        ditter = target;
        _coroutine = null;
        yield return null;

    }
}
