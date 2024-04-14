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
    public ResourcesStruct resourcesSell;
    
    [Header("Unit")]
    [SerializeField]
    public UnitScriptableObjects UnitAdd;
    
}

public enum WhichBudynek
{
    // jak to coœ bêdzie dodawane, to trzeba równie¿ dodaæ tag (o takiej samej nazwie) do unity (tagi do gameobjectów (tak jak Layer`y))
    // przy zmianie nazwy równie¿ tak¿e trzeba zaktualizowaæ
    archerBarrack,
    knightBarrack,
    peasantBarrack,
    Stable,
    StoneMine

}