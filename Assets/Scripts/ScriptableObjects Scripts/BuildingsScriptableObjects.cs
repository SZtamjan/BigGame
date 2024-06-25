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
    public Sprite buildingImage;
    
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
    // jak to coś będzie dodawane, to trzeba również dodać tag (o takiej samej nazwie) do unity (tagi do gameobjectów (tak jak Layer`y))
    // przy zmianie nazwy również także trzeba zaktualizować
    archerBarrack,
    knightBarrack,
    House,
    Stable,
    StoneMine,
    Sawmill,
    Farm,
    Tavern,
    Las

}