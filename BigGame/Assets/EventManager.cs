using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public delegate void BuildingActions();
    public static event BuildingActions BuildingAction;

    public static EventManager Instance;
    private void Awake()
    {
        Instance = this;
    }

    public void BuldingsActions()
    {
        if (BuildingAction!=null)
        {
            BuildingAction();
        }
    }
}
