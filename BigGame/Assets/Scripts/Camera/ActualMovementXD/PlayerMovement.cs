using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{


    [Header("Settings")]
    public float keySpeed = 6f; // predkosc przesuwania kamery WSADem
    public float groundDrag; // Tarcie - jak szybko sie zatrzyma
    public float mouseSpeed = 1f;// predkosc przesuwania kamery myszka
    public float camHeight = 3f; // warto�� sta�a pozycji kamery w osi Y

    [Header("Limiters")]
    public GameObject limitLeft;
    public GameObject limitRight;
    private float minX; // minimalna pozycja kamery w osi X
    private float maxX; // maksymalna pozycja kamery w osi X
    public float minZ; // minimalna pozycja kamery w osi Z
    public float maxZ; // maksymalna pozycja kamery w osi Z


    private bool isDragging = false; // flaga informujaca, czy uzytkownik przesuwa kamera
    private Vector3 lastMousePosition; // pozycja myszy podczas ostatniego klatkowania

    float horizontalInput;
    float verticalInput;

    Vector3 moveDirection;

    Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.drag = groundDrag;
        SetLimits();
    }

    private void Update()
    {
        // Poruszanie się myszką
        if (Input.GetMouseButtonDown(1) && !Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.S) && !Input.GetKey(KeyCode.D))
        {
            lastMousePosition = Input.mousePosition;
            isDragging = true;
        }
        else if (Input.GetMouseButtonUp(1))
        {
            isDragging = false;
        }

        if (isDragging)
        {
            // Obliczanie wektora przesunięcia kamery
            Vector3 delta = Input.mousePosition - lastMousePosition;
            Vector3 cameraMovement = new Vector3(delta.x, 0, delta.y) * Time.deltaTime * -mouseSpeed;

            // Aktualizowanie pozycji kamery
            transform.position += cameraMovement;

            // Ograniczanie pozycji kamery do okre�lonego zakresu
            float x = Mathf.Clamp(transform.position.x, minX, maxX);
            float z = Mathf.Clamp(transform.position.z, minZ, maxZ);
            transform.position = new Vector3(x, camHeight, z);

            lastMousePosition = Input.mousePosition;
        }

        //Poruszanie się WSAD

        MyInput();
        SpeedControl();
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    private void MovePlayer()
    {
        moveDirection.z = verticalInput; 
        moveDirection.x = horizontalInput;

        rb.AddForce(moveDirection.normalized * keySpeed * 10f, ForceMode.Force);

        float x = Mathf.Clamp(transform.position.x, minX, maxX);
        float z = Mathf.Clamp(transform.position.z, minZ, maxZ);
        transform.position = new Vector3(x, camHeight, z);
    }

    private void MyInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");
    }
    private void SpeedControl()
    {
        Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        //limit velocity if needed
        if (flatVel.magnitude > keySpeed)
        {
            Vector3 limitedVel = flatVel.normalized * keySpeed;
            rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
        }
    }
    
    private void SetLimits()
    {
        minX = limitLeft.transform.position.x;
        maxX = limitRight.transform.position.x;
    }

}
