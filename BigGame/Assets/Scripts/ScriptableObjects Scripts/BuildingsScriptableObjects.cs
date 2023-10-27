using UnityEngine;


[CreateAssetMenu(fileName = "ScriptableObjects", menuName = "ScriptableObjects/BuildingsStats")]
public class BuildingsScriptableObjects : ScriptableObject
{
    //Prefabs
    [Header("Budynek ")]
    public GameObject Budynek;
    public WhichBudynek whichBudynek;
    
    //Card Stats
    [Header("Statystyki ")]
    [SerializeField]
    public int cost = 5;
    [SerializeField]
    public new string name = "domek";
    [SerializeField]
    public string desc = "+5 do hajsu na ture";
    [SerializeField]
    public int moneyGain = 0;
}

public enum WhichBudynek
{
    koszary,
    tawerna,
    jakisTrzeci
}