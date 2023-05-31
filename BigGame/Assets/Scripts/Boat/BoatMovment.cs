using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatMovment : MonoBehaviour
{
    public float diameter = 10f; // �rednica okr�gu
    public float speedMax = 2f; // Pr�dko�� poruszania si� obiektu
    public float speedMin = 0.1f; // Pr�dko�� poruszania si� obiektu
    public float speed = 0.1f; // Pr�dko�� poruszania si� obiektu
    public float rotationSpeed = 1f;
    private Vector3 center; // �rodek okr�gu
    private float angle = 0f; // K�t wok� okr�gu
    private Vector3 targetPosition; // Aktualna pozycja docelowa

    private void Start()
    {
        center = transform.position; // Ustawienie �rodka okr�gu na pocz�tkow� pozycj� obiektu
        SetNewTargetPosition(); // Ustawienie pierwszej pozycji docelowej
    }

    private void Update()
    {
        // Obliczenie kierunku do celu
        Vector3 direction = targetPosition - transform.position;
        direction.y = 0f; // Ustawienie warto�ci Y na 0, aby obr�t odbywa� si� tylko w p�aszczy�nie poziomej

        if (direction != Vector3.zero)
        {
            // Obr�t obiektu w stron� celu
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }

        // Poruszanie si� obiektu w kierunku docelowym
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

        // Sprawdzenie, czy obiekt dotar� do pozycji docelowej
        if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
        {
            SetNewTargetPosition(); // Wyb�r nowej pozycji docelowej
        }
    }

    private void SetNewTargetPosition()
    {
        // Obliczenie losowego k�ta dla nowej pozycji docelowej
        float randomAngle = Random.Range(0f, 60f);
        speed = Random.Range(speedMin, speedMax);

        // Obliczenie pozycji na okr�gu na podstawie losowego k�ta i �rodka
        targetPosition = center + new Vector3(Mathf.Sin(randomAngle) * diameter / 2f, 0f, Mathf.Cos(randomAngle) * diameter / 2f);
    }
}