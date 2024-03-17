using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldUIRotator : MonoBehaviour
{
    [Header("Rotation Towards")]
    private Transform me;
    private Quaternion rotOffset = Quaternion.Euler(0f, 0f, 0f);

    private void Awake()
    {
        me = Camera.main.transform; 
        gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        Vector3 dir = me.position - transform.position;
        Quaternion whereLook = Quaternion.LookRotation(dir);
        whereLook *= rotOffset;
        transform.rotation = whereLook;
    }

    void Update()
    {
        Vector3 dir = me.position - transform.position;
        Quaternion whereLook = Quaternion.LookRotation(dir);
        whereLook *= rotOffset;
        transform.rotation = Quaternion.Slerp(transform.rotation, whereLook, Time.deltaTime*20f);
    }

    private void Rotate()
    {
        
    }
}
