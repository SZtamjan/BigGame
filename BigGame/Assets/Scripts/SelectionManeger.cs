using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionManeger : MonoBehaviour
{

    [SerializeField]
    private Camera mainCamera;

    public LayerMask selectionMask;

    public HexGrid hexGrid;

    List<Vector3Int> neighbours = new List<Vector3Int>();

    private void Awake()
    {
       
        if (mainCamera==null)
        {
            mainCamera = Camera.main;
        }
    }

    public void HandleClick(Vector3 mousePosition)
    {
        GameObject result;
        if (FindTarget(mousePosition,out result))
        {
            Hex selectedHex = result.GetComponent<Hex>();

           

            Debug.Log($"neighbours For {selectedHex.HexCoords} are: ");
            foreach (var neighboursPos in neighbours)
            {
                Debug.Log(neighboursPos);

            }
        }
    }

    private bool FindTarget(Vector3 mousePosition, out GameObject result)
    {
        RaycastHit hit;
        Ray ray = mainCamera.ScreenPointToRay(mousePosition);
        if (Physics.Raycast(ray,out hit,selectionMask))
        {
            result = hit.collider.gameObject;
            return true;
        }
        result = null;
        return false;
    }
}
