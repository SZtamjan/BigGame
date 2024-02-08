using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomHex : MonoBehaviour
{
    public RandomHexScriptable SO;


    private void ReplaceMe()
    {
        Transform transform = gameObject.transform;
        GameObject newHex = SO.RandomHex();
        if (newHex != null)
        {
            Instantiate(newHex, transform.position, transform.rotation, transform.parent);
        }
        else
        {
            string xd = "jest ze mno problem: ";
            string myName = gameObject.name;
            Debug.Log($"{xd} {transform.position} {myName}");
        }
    }

    private void Start()
    {
        ReplaceMe();
        Destroy(gameObject);
    }

    [Button]
    public void ReplaceWithHex()
    {
        ReplaceMe();
        DestroyImmediate(gameObject);
    }
}
