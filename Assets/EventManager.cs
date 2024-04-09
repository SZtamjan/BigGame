using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public delegate void BuildingActions();
    public static event BuildingActions BuildingAction;

    public delegate void NewPlayerTurnEvent();

    public static event NewPlayerTurnEvent NewPlayerTurn;

    public static EventManager Instance;
    private void Awake()
    {
        Instance = this;
    }

    public void BuldingsActions()
    {
        BuildingAction?.Invoke();
    }

    public void NewPlayerTurnFunc()
    {
        NewPlayerTurn?.Invoke();
    }
}
