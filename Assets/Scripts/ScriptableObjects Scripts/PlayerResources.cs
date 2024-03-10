using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerResources", menuName = "ScriptableObjects/PlayerResources")]
public class PlayerResources : ScriptableObject
{
    [Header("Surowce")]
    public int gold;
    public int stone;
    public int wood;
    public int food;
    
    //Prefabs
    [Header("Akcje")]
    public int cashCastleOnTurn = 10;
    
    
}
