using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopUpDisplayBuildingInfo : MonoBehaviour
{
    private Coroutine followCor;
    private void OnEnable()
    {
        followCor = StartCoroutine(FollowCursor());
    }

    private void OnDisable()
    {
        if(followCor != null) StopCoroutine(FollowCursor());
    }


    private IEnumerator FollowCursor()
    {
        
        yield return null;
    }
}
