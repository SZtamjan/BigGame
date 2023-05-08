using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolTip : MonoBehaviour
{
    // Start is called before the first frame update
    public static ToolTip instance;
    [SerializeField] public Vector3 offset;
    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        gameObject.SetActive(false);
    }
    public void ActivateToolTip()
    {
        gameObject.SetActive(true);
    }
    public void DeactivateToolTip()
    { 
        gameObject.SetActive(false);
    }
}
