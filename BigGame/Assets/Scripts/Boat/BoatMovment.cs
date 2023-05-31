using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatMovment : MonoBehaviour
{
    public float diameter = 10f; // Œrednica okrêgu
    public float speedMax = 2f; // Prêdkoœæ poruszania siê obiektu
    public float speedMin = 0.1f; // Prêdkoœæ poruszania siê obiektu
    public float speed = 0.1f; // Prêdkoœæ poruszania siê obiektu
    public float rotationSpeed = 1f;
    private Vector3 center; // Œrodek okrêgu
    private float angle = 0f; // K¹t wokó³ okrêgu
    private Vector3 targetPosition; // Aktualna pozycja docelowa

    private void Start()
    {
        center = transform.position; // Ustawienie œrodka okrêgu na pocz¹tkow¹ pozycjê obiektu
        SetNewTargetPosition(); // Ustawienie pierwszej pozycji docelowej
    }

    private void Update()
    {
        // Obliczenie kierunku do celu
        Vector3 direction = targetPosition - transform.position;
        direction.y = 0f; // Ustawienie wartoœci Y na 0, aby obrót odbywa³ siê tylko w p³aszczyŸnie poziomej

        if (direction != Vector3.zero)
        {
            // Obrót obiektu w stronê celu
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }

        // Poruszanie siê obiektu w kierunku docelowym
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

        // Sprawdzenie, czy obiekt dotar³ do pozycji docelowej
        if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
        {
            SetNewTargetPosition(); // Wybór nowej pozycji docelowej
        }
    }

    private void SetNewTargetPosition()
    {
        // Obliczenie losowego k¹ta dla nowej pozycji docelowej
        float randomAngle = Random.Range(0f, 60f);
        speed = Random.Range(speedMin, speedMax);

        // Obliczenie pozycji na okrêgu na podstawie losowego k¹ta i œrodka
        targetPosition = center + new Vector3(Mathf.Sin(randomAngle) * diameter / 2f, 0f, Mathf.Cos(randomAngle) * diameter / 2f);
    }
}