using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DepthFixer : MonoBehaviour
{
    [SerializeField] private Camera cam;

    private void Start()
    {
        //If this is off, then OnMouse events doesn't work
        //e.g. without this WUI doesn't work
        //It has to be set on UI Camera
        cam.clearFlags = CameraClearFlags.Depth;
    }
}
