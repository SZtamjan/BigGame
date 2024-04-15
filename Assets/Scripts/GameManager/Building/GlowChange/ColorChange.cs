using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorChange : MonoBehaviour
{
    [SerializeField] public ColorChangeRules colorRules;
    public static ColorChange Instance;
    void Awake()
    {
        Instance = this;
    }

    
}
