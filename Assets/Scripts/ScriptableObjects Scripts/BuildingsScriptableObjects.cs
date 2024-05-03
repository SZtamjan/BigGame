using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;


[CreateAssetMenu(fileName = "ScriptableObjects", menuName = "ScriptableObjects/Buildings/BuildingsStats")]
public class BuildingsScriptableObjects : ScriptableObject
{
    //General info
    [Header("General info")]
    public GameObject budynekPrefab;
    public WhichBudynek whichBudynek; //Used for limiting building type per level
    
    //Card Stats
    [Header("Info")]
    [SerializeField]
    public new string name = "domek";
    [SerializeField]
    public string desc = "+5 do hajsu na ture";

    [Header("Statystyki budynku")]
    [SerializeField]
    public List<UpdateBuildingStruct> buildingLevelsList;
}

public enum WhichBudynek //Used for limiting building type per level
{
    // jak to co� b�dzie dodawane, to trzeba r�wnie� doda� tag (o takiej samej nazwie) do unity (tagi do gameobject�w (tak jak Layer`y))
    // przy zmianie nazwy r�wnie� tak�e trzeba zaktualizowa�
    archerBarrack,
    knightBarrack,
    peasantBarrack,
    Stable,
    StoneMine

}