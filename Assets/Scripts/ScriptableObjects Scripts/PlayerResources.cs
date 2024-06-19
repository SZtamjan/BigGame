using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerResources", menuName = "ScriptableObjects/PlayerResources")]
public class PlayerResources : ScriptableObject
{
    [Header("Surowce na start")]
    public ResourcesStruct playerResources;
    
    //Prefabs
    [Header("Akcje")]
    public ResourcesStruct resourcesOnTurn;


}
