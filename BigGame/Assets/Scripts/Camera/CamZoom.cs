using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamZoom : MonoBehaviour
{    
   
    public float zoomSpeed = 2.0f;
    public float zoomMin = 1.0f;
    public float zoomMax = 20.0f;

    private float currentZoom = 0.0f;

    private void Start()
    {
        currentZoom = Camera.main.fieldOfView;
    }

    private void Update()
    {
        float scrollWheelInput = Input.GetAxis("Mouse ScrollWheel");

        if (scrollWheelInput != 0)
        {
            currentZoom -= scrollWheelInput * zoomSpeed;

            // Clamp the zoom level to zoomMin and zoomMax
            currentZoom = Mathf.Clamp(currentZoom, zoomMin, zoomMax);
        }

        // Set the camera's field of view based on the currentZoom value
        Camera.main.fieldOfView = Mathf.Lerp(Camera.main.fieldOfView, currentZoom, Time.deltaTime * zoomSpeed);
    }

}
