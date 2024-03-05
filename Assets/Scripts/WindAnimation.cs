using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindAnimation : MonoBehaviour
{
    public float maxAngle = 10f; // Maksymalny k�t rotacji drzewa
    public float speed = 1f; // Szybko�� animacji
    public float intensity = 1f; // Intensywno�� animacji
    public Vector3 windDirection = Vector3.right; // Kierunek wiatru
    public float noiseFrequency = 0.1f; // Cz�stotliwo�� Perlin Noise
    public float branchFactor = 1f; // Wsp�czynnik zmiennego ruchu ga��zi

    private Quaternion baseRotation; // Pocz�tkowa rotacja drzewa
    private float randomOffset; // Losowe przesuni�cie

    void Start()
    {
        // Zapisanie pocz�tkowej rotacji drzewa
        baseRotation = transform.rotation;

        // Wygenerowanie losowego przesuni�cia
        randomOffset = Random.Range(0f, 100f);
    }

    void Update()
    {
        // Animacja wietrzenia
        float noiseValue = Mathf.PerlinNoise(Time.time * speed * intensity, randomOffset);
        float angle = Mathf.Lerp(-maxAngle, maxAngle, noiseValue);
        Quaternion targetRotation = Quaternion.Euler(baseRotation.eulerAngles + new Vector3(angle, 0, 0));

        // Zastosowanie zmiennej rotacji ga��zi
        for (int i = 0; i < transform.childCount; i++)
        {
            Transform branch = transform.GetChild(i);
            float branchOffset = Mathf.PerlinNoise(Time.time * speed * intensity * branchFactor, randomOffset) * 2 - 1;
            Quaternion branchRotation = Quaternion.Euler(branch.localRotation.eulerAngles + new Vector3(angle * branchOffset, 0, 0));
            branch.localRotation = Quaternion.Lerp(branch.localRotation, branchRotation, Time.deltaTime * 5f);
        }

        // Dodatkowa rotacja w zale�no�ci od kierunku wiatru
        Quaternion windRotation = Quaternion.FromToRotation(Vector3.up, windDirection);
        targetRotation *= windRotation;

        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * 5f);
    }
}
