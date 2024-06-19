using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReplaceHex : MonoBehaviour
{
    [SerializeField] private GameObject replaceMeWith;
    
    public IEnumerator ReplaceMe()
    {
        yield return new WaitForSeconds(.3f);
        GameObject dwa = Instantiate(replaceMeWith, transform);
        dwa.transform.parent = transform.parent.transform.parent;
        Destroy(gameObject);
        yield return null;
    }
}
