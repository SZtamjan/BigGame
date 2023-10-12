using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static CastleClass;
using static GameManager;

public class CastleStats : MonoBehaviour
{


    [Header("Statystyki ")]
    [SerializeField]
    int hp = 100;

    public List<CastleArmaments> defenceArmaments;

    public bool isMyCastle = true;


    private void Start()
    {
        UIController.Instance.CastleHpSetMaxHealth(hp, isMyCastle);
        defenceArmaments = new List<CastleArmaments>();
        
    }

    public int ReturnHp()
    {
        return hp;
    }

    public void AddDefenceArmaments(CastleArmaments arnaments)
    {
        defenceArmaments.Add(arnaments);
    }

    public void DamageTaken(int obtained)
    {
        hp -= obtained;

        if (hp <= 0)
        {
            Debug.Log("KoniecGry");
            if (!isMyCastle)
            {
                instance.UpdateGameState(GameState.Victory);
            }
            else
            {
                instance.UpdateGameState(GameState.Lose);
            }
        }

        UIController.Instance.CastleHpSetHealth(hp, isMyCastle);

    }


}
