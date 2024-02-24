using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// podkradni�te z t�d https://youtu.be/htZijEO7ZmE

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
