using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CastleStats : MonoBehaviour
{
    [System.Serializable]
    public class Defences
    {
        int damage;
        int reach;
    }

    


    [Header("Statystyki ")]
    [SerializeField]
    int hp = 100;

    public List<Defences> defenceArmaments;


    private void Start()
    {
        defenceArmaments = new List<Defences>();
    }

    public int ReturnHp()
    {
        return hp;
    }

    public void addDefenceArmaments(Defences arnaments)
    {
        defenceArmaments.Add(arnaments);
    }



}
