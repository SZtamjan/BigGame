using UnityEngine;


[CreateAssetMenu(fileName = "ScriptableObjects", menuName = "ScriptableObjects/UnitStats")]
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

    //Card Stats
    [Header("Karta ")]
    [SerializeField]
    public int cost = 5;
    [SerializeField]
    public new string name = "Karta wpierdolu";
    [SerializeField]
    public string desc = "To naprawdê karta wpierdolu";
    [SerializeField]
    public Sprite artwork;
}
