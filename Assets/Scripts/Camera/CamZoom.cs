using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamZoom : MonoBehaviour
{
    private Camera mainCam;
    public float zoomSpeed = 15.0f;
    public float zoomMin = 60.0f;
    public float zoomMax = 90.0f;

    private float currentZoom = 0.0f;

    private bool inMenu = false;

    public void ChangeZoomLock()
    {
        inMenu = inMenu == false;
    }

    private void Start()
    {
        mainCam = Camera.main;
        currentZoom = mainCam.fieldOfView;
    }

    private void Update()
    {
        float scrollWheelInput = Input.GetAxis("Mouse ScrollWheel");

        if (scrollWheelInput != 0 && !inMenu)
        {
            currentZoom -= scrollWheelInput * zoomSpeed;

            currentZoom = Mathf.Clamp(currentZoom, zoomMin, zoomMax);
        }

        mainCam.fieldOfView = Mathf.Lerp(mainCam.fieldOfView, currentZoom, Time.deltaTime * zoomSpeed);
    }
}
