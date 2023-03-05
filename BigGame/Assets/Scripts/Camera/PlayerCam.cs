using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCam : MonoBehaviour
{
    public float sensX;
    public float sensY;

    public Transform orientation;

    float xRotation;
    float yRotation;


    //Zoom
    /*
    public float zoomSpeed = 1.0f;
    public float minFOV = 10.0f;
    public float maxFOV = 60.0f;
    */

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * sensX;
        float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * sensY;

        yRotation += mouseX;

        xRotation -= mouseY;


        //Limit rotation - static
        xRotation = Mathf.Clamp(xRotation, 60f, 60f);
        yRotation = Mathf.Clamp(yRotation, 0f, 0f);

        transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
        orientation.rotation = Quaternion.Euler(0, yRotation, 0);




        //Zoom
        /*
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll != 0)
        {
            float fov = Camera.main.fieldOfView;
            fov -= scroll * zoomSpeed;
            fov = Mathf.Clamp(fov, minFOV, maxFOV);
            Camera.main.fieldOfView = fov;
        }*/


    }




}
