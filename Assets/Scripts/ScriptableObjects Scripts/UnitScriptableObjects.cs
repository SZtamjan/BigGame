using System;
using UnityEngine;
using UnityEngine.Serialization;

//[Serializable]
[CreateAssetMenu(fileName = "UnitSO", menuName = "ScriptableObjects/UnitStats")]
public class UnitScriptableObjects : ScriptableObject
{
    //Prefabs
    [Header("Jednostka ")]
    public GameObject unit;

    //Staty Jednostki
    [Header("Statystyki ")]
    [SerializeField]
    public int hp = 5;
    [SerializeField]
    public int damage = 2;
    [SerializeField]
    public int movmentDistance = 1;
    [SerializeField]
    public int attackReach = 1;
    [SerializeField]
    public bool playersUnit = true;
    [SerializeField]
    public float moveSpeed = 0.5f;

    //Card info
    [Header("Karta ")]
    [SerializeField]
    public new string name = "Karta wpierdolu";
    [SerializeField]
    public string desc = "To naprawdę karta wpierdolu";
    [SerializeField]
    public Sprite artwork;
    
    //Cost
    [Header("Koszt karty")] 
    [Tooltip("Wartosc 0 nie bedzie brana pod uwage podczas sprawdzania")] 
    public ResourcesStruct resources;
    
}
