using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// podkradniête z t¹d https://youtu.be/htZijEO7ZmE


public class HexCoordinates : MonoBehaviour
{
    public static float xOffset = 0.86f, yOffset = 1, zOffset = 0.75f;

    internal Vector3Int GetHexCoords()=>offsetCoordinates;
    

    [Header("Offset coordinates")]
    [SerializeField]
    private Vector3Int offsetCoordinates;



    private void Awake()
    {
        offsetCoordinates = ConvetPositionToOffset(transform.position);
    }

    private Vector3Int ConvetPositionToOffset(Vector3 position)
    {
        int x = Mathf.CeilToInt(position.x / xOffset);
        int y = Mathf.CeilToInt(position.y / yOffset);
        int z = Mathf.CeilToInt(position.z / zOffset);
        return new Vector3Int(x, y, z);
    }
}
