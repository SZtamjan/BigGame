using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public delegate void BuildingActions();
    public static event BuildingActions BuildingAction;

    public delegate void BuildingsColorChange(WhichBudynek? type);
    public static event BuildingsColorChange BuildingColorChange;

    
    public delegate void NewPlayerTurnEvent();

    public static event NewPlayerTurnEvent NewPlayerTurn;

    public static EventManager Instance;
    private void Awake()
    {
        Instance = this;
    }

    public void NewPlayerTurnFunc()
    {
        BuildingAction?.Invoke();
        NewPlayerTurn?.Invoke();
    }

    public void BuldingColorChange(WhichBudynek? type)
    {
        if (type==null)
        {
            Debug.Log($"invoke BuldingColorChange(null)");           
        }
        else
        {
            Debug.Log($"invoke BuldingColorChange({type})");
        }
        BuildingColorChange?.Invoke(type);
    }

    


}
