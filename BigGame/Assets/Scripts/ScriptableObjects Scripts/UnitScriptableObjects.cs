using UnityEngine;


[CreateAssetMenu(fileName = "UnitScriptableObjects", menuName = "UnitScriptableObjects/UnitStats")]
public class UnitScriptableObjects : ScriptableObject
{
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
}
