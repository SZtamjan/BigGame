using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "ScriptableObjects", menuName = "ScriptableObjects/RandomHex")]
public class RandomHexScriptable : ScriptableObject
{
    public List<GameObject> hexs;

    public GameObject RandomHex()
    {
        if (hexs.Count == 0)
        {
            return null;
        }
        int i = Random.Range(0, hexs.Count);
        return hexs[i];
    }
}
