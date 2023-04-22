using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed;
    public float groundDrag;

    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask whatIsGround;
    bool grounded = true;


    [Header("RMB Movement")]
    public float movementSpeed = 50.0f;

    [Header("tmp limiters")]
    public float keySpeed = 20f; // pr�dko�� przesuwania kamery WSADem
    public float mouseSpeed = 1f;// pr�dko�� przesuwania kamery myszk�
    public float minX = -5f; // minimalna pozycja kamery w osi X
    public float maxX = 5f; // maksymalna pozycja kamery w osi X
    public float minZ = -5f; // minimalna pozycja kamery w osi Z
    public float maxZ = 5f; // maksymalna pozycja kamery w osi Z
    public float fixedY = 3f; // warto�� sta�a pozycji kamery w osi Y


    private bool isDragging = false; // flaga informuj�ca, czy u�ytkownik przesuwa kamer�
    private Vector3 lastMousePosition; // pozycja myszy podczas ostatniego klatkowania

    float horizontalInput;
    float verticalInput;

    Vector3 moveDirection;

    Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
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
            float x = Mathf.Clamp(transform.position.x, -5, 5);
            float z = Mathf.Clamp(transform.position.z, -5, 5);
            transform.position = new Vector3(x, transform.position.y, z);

            lastMousePosition = Input.mousePosition;
        }

        //Poruszanie się WSAD
        //handle drag
        if (grounded)
            rb.drag = groundDrag;
        else
            rb.drag = 0;

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

        rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);

        float x = Mathf.Clamp(transform.position.x, -5, 5);
        float z = Mathf.Clamp(transform.position.z, -5, 5);
        transform.position = new Vector3(x, transform.position.y, z);
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
        if (flatVel.magnitude > moveSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * moveSpeed;
            rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
        }
    }
}
