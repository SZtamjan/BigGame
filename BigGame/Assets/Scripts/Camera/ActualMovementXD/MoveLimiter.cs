using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveLimiter : MonoBehaviour
{
    public void OnCollisionStay(Collision collision)
    {
        gameObject.GetComponent<PlayerMovement>().JustCollided();
    }
}
