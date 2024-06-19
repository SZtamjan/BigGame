using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CardScriptableObject : ScriptableObject
{
    //Card info
    [Header("Karta ")]
    [SerializeField]
    public new string name = "Karta wpierdolu";

    [SerializeField]
    public string desc = "To naprawdę karta wpierdolu";
    [SerializeField]
    public Sprite artwork;

    [SerializeField]
    public bool playersSide = true;

    //Cost
    [Header("Koszt karty")]
    [Tooltip("Wartosc 0 nie bedzie brana pod uwage podczas sprawdzania")]
    public ResourcesStruct resources;

    public abstract object GetStats();

    public abstract void CardAction(GameObject karta, CardScriptableObject stats);

    public abstract int[] GetStatsCard();
}