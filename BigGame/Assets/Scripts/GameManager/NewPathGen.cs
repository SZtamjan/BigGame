using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewPathGen : MonoBehaviour
{
    public GameObject zamek;
    public LayerMask lejer;
    public float radius = 1f;

    public void Detect()
    {
        // Wykrywanie obiektów wokół bieżącego obiektu za pomocą OverlapSphere
        Collider[] colliders = Physics.OverlapSphere(zamek.transform.position, radius, lejer);

        // Przetwarzanie wykrytych obiektów
        foreach (Collider collider in colliders)
        {
            // Tutaj możesz wykonać działania na wykrytych obiektach
            Debug.Log("Wykryto obiekt: " + collider.gameObject.name);
        }
    }
}
