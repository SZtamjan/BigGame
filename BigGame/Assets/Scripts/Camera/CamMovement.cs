using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamMovement : MonoBehaviour
{

    //To jakoœ dziwnie dzia³a, odrazu siê wykonuje chyba
    //Wyniki https://www.youtube.com/results?search_query=unity+steering+camera+with+buttons
    //Strategy game camera tut https://www.youtube.com/watch?v=3Y7TFN_DsoI
    //The one that I just used https://www.youtube.com/watch?v=uaiMvAK6Y0g
    //Guy from india xd https://youtu.be/lYIRm4QEqro - probably won't work cuz no limit rotation

    /*
    float inputX, inputY, inputZ;

    // Update is called once per frame
    void Update()
    {
          inputX = Input.GetAxis("CamLeft");
          inputY = Input.GetAxis("CamUp");
          inputZ = Input.GetAxis("CamMoveL");

          if (inputX != 0)
              rotateY();
          if (inputY != 0)
              rotateX();
          if (inputZ != 0)
              move();
}
private void rotateY()
    {
        transform.Rotate(new Vector3(0f, inputX * Time.deltaTime, 0f));
    }
    private void rotateX()
    {
        transform.Rotate(new Vector3(inputY * Time.deltaTime, 0f, 0f));
    }
    private void move()
    {
        
    }*/
}
