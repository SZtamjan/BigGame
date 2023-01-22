using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static CastleClass;

public class CastleStats : MonoBehaviour
{


    [Header("Statystyki ")]
    [SerializeField]
    int hp = 100;

    public List<CastleArmaments> defenceArmaments;


    private void Start()
    {
        defenceArmaments = new List<CastleArmaments>();
    }

    public int ReturnHp()
    {
        return hp;
    }

    public void addDefenceArmaments(CastleArmaments arnaments)
    {
        defenceArmaments.Add(arnaments);
    }



}
