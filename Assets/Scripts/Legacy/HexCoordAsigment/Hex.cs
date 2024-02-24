using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// podkradniête z t¹d https://youtu.be/htZijEO7ZmE

[SelectionBase]
public class Hex : MonoBehaviour
{
    [SerializeField]   
    private HexCoordinates hexCoordinates;

    public Vector3Int HexCoords => hexCoordinates.GetHexCoords();

    private void Awake()
    {
        hexCoordinates = GetComponent<HexCoordinates>();
       
    }
    
    

}
