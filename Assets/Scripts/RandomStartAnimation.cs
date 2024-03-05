using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomStartAnimation : MonoBehaviour
{
    public Animation[] animationComponents; // Tablica referencji do komponent�w animacji
    private int randomIndex; // Indeks losowej animacji

    void Start()
    {
        // Sprawdzenie, czy przypisano animacje
        if (animationComponents == null || animationComponents.Length == 0)
        {
            Debug.LogError("Animation components not assigned!");
            return;
        }

        // Wyb�r losowej animacji
        randomIndex = Random.Range(0, animationComponents.Length);

        // Sprawdzenie, czy indeks jest w zakresie
        if (randomIndex >= 0 && randomIndex < animationComponents.Length)
        {
            Animation selectedAnimation = animationComponents[randomIndex];

            // Sprawdzenie, czy komponent animacji zosta� przypisany
            if (selectedAnimation == null)
            {
                Debug.LogError("Selected animation component is null!");
                return;
            }

            // Wyb�r losowej klatki animacji
            float randomTime = Random.Range(0f, selectedAnimation.clip.length);
            selectedAnimation[selectedAnimation.clip.name].time = randomTime;
            selectedAnimation.Play();
        }
        else
        {
            Debug.LogError("Random index out of range!");
        }
    }
}
