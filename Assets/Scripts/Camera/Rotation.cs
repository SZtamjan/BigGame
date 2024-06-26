using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotation : MonoBehaviour
{
    public float sensX;
    public float sensY;

    float xRotation;
    float yRotation;

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
    }




}
