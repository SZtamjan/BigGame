using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerCash", menuName = "UnitScriptableObjects/PlayerCash")]
public class PlayerCash : ScriptableObject
{
    //Prefabs
    [Header("Cash gracza ")]
    public int playerCash = 100;
    public int cashCastleOnTurn = 10;
}
