using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[SelectionBase]
public class Castle : MonoBehaviour
{


    [Tag] public string targetTag = "Gate";
    public float searchRadius = 5f;

    public List<Gate> gates = new List<Gate>();

    [SerializeField] private bool isPlayerSide = false;

    [SerializeField] private int MaxHp = 100;
    private int _hp = 100;

    public int HpChange
    {
        get
        {
            return (int)_hp;
        }
        set
        {
            _hp = value;
            UIController.Instance.CastleHpSetHealth(_hp, isPlayerSide);
        }
    }
    #region pathGeneration
    public void GatesInitialization()
    {
        GetGates();
        SetGates();
        SetPaths();
    }

    public void GetGates()
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

    public void SetGates()
    {

        foreach (Gate gate in gates)
        {
            gate.SetIfplayerOrNot(isPlayerSide);
            gate.SetMyCastle(this);

        }
    }

    public void SetPaths()
    {
        foreach (var gate in gates)
        {
            gate.GeneratePath();
        }
    }

    #endregion



    private void Start()
    {
        UIController.Instance.CastleHpSetMaxHealth(MaxHp, isPlayerSide);
        HpChange = MaxHp;

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
