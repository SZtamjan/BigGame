using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[SelectionBase]
public class Castle : MonoBehaviour
{

    public string targetTag = "Gate";
    public float searchRadius = 5f;

    public List<Gate> gates = new List<Gate>();

    [SerializeField] private bool isPlayerSide = false;

    private void Awake()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, searchRadius);
        foreach (Collider collider in colliders)
        {
            if (collider.CompareTag(targetTag))
            {
                gates.Add(collider.GetComponent<Gate>());
            }
        }
    }
    private void Start()
    {
        if (isPlayerSide)
        {
            CastlesController.Instance.playerCastle = this;
        }
        else
        {
            CastlesController.Instance.enemyCastle = this;
        }

    }

}
