using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// podkradniête z t¹d https://youtu.be/htZijEO7ZmE

public class HexGrid : MonoBehaviour
{
    Dictionary<Vector3Int, Hex> hexTileDict = new Dictionary<Vector3Int, Hex>();
    Dictionary<Vector3Int, List<Vector3Int>> hexTileNeighbourDict = new Dictionary<Vector3Int, List<Vector3Int>>();

    void Start()
    {
        foreach (var hex in FindObjectsOfType<Hex>())
        {
            hexTileDict[hex.HexCoords] = hex;
        }
       

    }

    public Hex GetTileAt(Vector3Int hexCoordinates)
    {
        Hex result = null;
        hexTileDict.TryGetValue(hexCoordinates, out result);
        return result;
    }

    public List<Vector3Int> GetNeighborsFor(Vector3Int hexCoordinates)
    {
        if (hexTileDict.ContainsKey(hexCoordinates)==false)
        {
            return new List<Vector3Int>();
        }
        if (hexTileNeighbourDict.ContainsKey(hexCoordinates))
        {
            return hexTileNeighbourDict[hexCoordinates];
        }

        hexTileNeighbourDict.Add(hexCoordinates, new List<Vector3Int>());

        foreach (var direction in Direction.GetDirectionList(hexCoordinates.z))
        {
            if (hexTileDict.ContainsKey(hexCoordinates + direction))
            {
                hexTileNeighbourDict[hexCoordinates].Add(hexCoordinates + direction);
            }
        }
        return hexTileNeighbourDict[hexCoordinates];
    }



    public static class Direction
    {
        public static List<Vector3Int> directionOffsetOdd = new List<Vector3Int>
        {
            new Vector3Int(-1, 0, 1), // UP 1
            new Vector3Int(0, 0, 1), // UP 2
            new Vector3Int(1,0,0), // right
            new Vector3Int(0, 0, -1), // down 1
            new Vector3Int(-1, 0, -1), //down 2
            new Vector3Int(-1, 0, 0) //left
        };

        public static List<Vector3Int> directionOffsetEven = new List<Vector3Int>
        {
            new Vector3Int(0, 0, 1), // UP 1
            new Vector3Int(1, 0, 1), // UP 2
            new Vector3Int(1,0,0), // right
            new Vector3Int(1, 0, -1), // down 1
            new Vector3Int(0, 0, -1), //down 2
            new Vector3Int(-1, 0, 0) //left
        };



        public static List<Vector3Int> GetDirectionList(int z)
            => z % 2 == 0 ? directionOffsetEven : directionOffsetOdd;
        
    }

}
