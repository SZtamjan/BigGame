﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Serialization;

public class PlayerMovement : MonoBehaviour
{
    public static PlayerMovement instance;

    [Header("Settings")]
    public float keySpeed = 6f; // predkosc przesuwania kamery WSADem
    public float groundDrag; // Tarcie - jak szybko sie zatrzyma
    public float mouseSpeed = 1f;// predkosc przesuwania kamery myszka
    public float camHeight = 3f; // wartosc stala pozycji kamery w osi Y

    [Header("Math Limiters")]
    [Tooltip("Domyślnie zamek gracza")]
    public GameObject limitLeft;
    [Tooltip("Domyślnie zamek Enemy")]
    public GameObject limitRight;
    [Tooltip("Trzeba wybrać samemu hexa")]
    public GameObject limitFront;
    [Tooltip("Trzeba wybrać samemu hexa")]
    public GameObject limitBack;
    private float minX; // minimalna pozycja kamery w osi X
    private float maxX; // maksymalna pozycja kamery w osi X
    private float minZ; // minimalna pozycja kamery w osi Z
    private float maxZ; // maksymalna pozycja kamery w osi Z

    [Header("Hitbox Limiters")]
    [SerializeField] private GameObject limiterLeft;
    [SerializeField] private GameObject limiterRight;
    [SerializeField] private GameObject limiterFront;
    [SerializeField] private GameObject limiterBack;
    [SerializeField] private float fixedPos = 0.01f;

    private bool isDragging = false; // flaga informujaca, czy uzytkownik przesuwa kamera
    private Vector3 lastMousePosition; // pozycja myszy podczas ostatniego klatkowania

    float horizontalInput;
    float verticalInput;

    Vector3 moveDirection;

    Rigidbody rb;

    private void Awake()
    {
        instance=this;
        rb = GetComponent<Rigidbody>();
        rb.drag = groundDrag;
    }
    public void CameraSetting()
    {
        
        SetLimiters();
        SetLimits();

        SetLimitersSize();
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

        rb.AddForce(moveDirection.normalized * (keySpeed * 10f), ForceMode.Force);

        if(minX == 0 && minZ == 0) return;
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

    private void SetLimiters()
    {
        minZ = limitBack.transform.position.z;
        maxZ = limitFront.transform.position.z;
        if (limitLeft == null)
        {
            limitLeft = CastlesController.Instance.playerCastle.gameObject;
        }
        
        if (limitRight == null)
        {
            limitRight = CastlesController.Instance.enemyCastle.gameObject;
        }
        //Left Limiter
        Vector3 limiterLeftPos = new Vector3(limitLeft.transform.position.x-fixedPos,camHeight,(limitFront.transform.position.z + limitBack.transform.position.z) / 2f);
        limiterLeft.transform.position = limiterLeftPos;
        
        //Right Limiter
        Vector3 limiterRightPos = new Vector3(limitRight.transform.position.x+fixedPos,camHeight,limiterLeftPos.z);
        limiterRight.transform.position = limiterRightPos;
        
        //Front Limiter
        Vector3 limiterFrontPos = new Vector3((limitLeft.transform.position.x + limitRight.transform.position.x) / 2f,
            camHeight, maxZ+fixedPos);
        limiterFront.transform.position = limiterFrontPos;
        
        //Back Limiter
        Vector3 limiterBackPos = new Vector3(limiterFrontPos.x,camHeight,limitBack.transform.position.z-fixedPos);
        limiterBack.transform.position = limiterBackPos;
    }

    private void SetLimitersSize()
    {
        float xSize = Math.Abs(limitLeft.transform.position.x - limitRight.transform.position.x);
        Vector3 newXScale = limiterFront.transform.localScale;
        newXScale.x = xSize;
        limiterFront.transform.localScale = newXScale;
        limiterBack.transform.localScale = newXScale;
        
        float zSize = Math.Abs(limitFront.transform.position.z - limitBack.transform.position.z);
        Vector3 newZScale = limiterLeft.transform.localScale;
        newZScale.z = zSize;
        limiterLeft.transform.localScale = newZScale;
        limiterRight.transform.localScale = newZScale;
    }
}
