using UnityEngine;
using UnityEngine.Serialization;


[CreateAssetMenu(fileName = "ScriptableObjects", menuName = "ScriptableObjects/Buildings/BuildingsStats")]
public class BuildingsScriptableObjects : ScriptableObject
{
    //Prefabs
    [Header("Budynek")]
    public GameObject Budynek;
    public WhichBudynek whichBudynek;
    
    //Card Stats
    [Header("Info")]
    [SerializeField]
    public new string name = "domek";
    [SerializeField]
    public string desc = "+5 do hajsu na ture";
    
    [Header("Statystyki ")]
    [SerializeField]
    public ResourcesStruct resourcesCost;
    [SerializeField] 
    public ResourcesStruct resourcesGainOnTurn;
    [SerializeField]
    public UnitScriptableObjects UnitAdd;
    
}

public enum WhichBudynek
{
    archerBarrack,
    knightBarrack,
    peasantBarrack,
    Stable,
    StoneMine

}