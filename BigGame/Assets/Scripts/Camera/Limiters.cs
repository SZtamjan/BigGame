using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Limiters : MonoBehaviour
{
    [SerializeField] private GameObject limiterLeft;
    [SerializeField] private GameObject limiterRight;
    [SerializeField] private GameObject limiterFront;
    [SerializeField] private GameObject limiterBack;

    public static Limiters Instance;

    private void Awake()
    {
        Instance = this;
    }

    public List<GameObject> GetLimiters()
    {
        List<GameObject> limiters = new List<GameObject>
        {
            limiterLeft,
            limiterRight,
            limiterFront,
            limiterBack
        };
        return limiters;
    }

}
