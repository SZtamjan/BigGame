using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindAnimation : MonoBehaviour
{
    public float maxAngle = 10f; // Maksymalny k¹t rotacji drzewa
    public float speed = 1f; // Szybkoœæ animacji

    private Quaternion baseRotation; // Pocz¹tkowa rotacja drzewa

    void Start()
    {
        // Zapisanie pocz¹tkowej rotacji drzewa
        baseRotation = transform.rotation;
    }

    void Update()
    {
        // Animacja wietrzenia
        float offset = Mathf.PerlinNoise(Time.time * speed, 0f) * 2 - 1;
        float angle = Mathf.Lerp(-maxAngle, maxAngle, offset);
        Quaternion targetRotation = Quaternion.Euler(baseRotation.eulerAngles + new Vector3(angle, 0, 0));
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * 5f);
    }
}
