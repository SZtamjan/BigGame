using NaughtyAttributes;
using System.Collections.Generic;
using UnityEngine;

public class ReplaceAllHexs : MonoBehaviour
{
    void Start()
    {

    }
    [Button]
    private void ReplaceHexs()
    {
        List<RandomHex> hexs = new List<RandomHex>();
        // Iterate through all children of this game object
        foreach (Transform child in transform)
        {
            // Check if the child has the component "RandomHex"
            RandomHex randomHexComponent = child.GetComponent<RandomHex>();

            if (randomHexComponent != null)
            {
                hexs.Add(randomHexComponent);
            }

            foreach (Transform grandChild in child)
            {
                RandomHex grandChildRandomHexComponent = grandChild.GetComponent<RandomHex>();
                if (grandChildRandomHexComponent != null)
                {
                    hexs.Add(grandChildRandomHexComponent);
                }
            }

        }
        for (int i = 0; i < hexs.Count; i++)
        {
            hexs[i].ReplaceWithHex();
        }

    }
}
