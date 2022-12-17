using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Kto zaczyna")]
    [SerializeField]
    public bool playerTurn = false;


    void GameManeger()
    {

    }

    public bool ReturnTurn()
    {
        return playerTurn;
    }

    public void PlayerTurnEnd()
    {
        
        playerTurn = false;
        GetComponent<PatchControler>().PlayerUnitMove();
        

    }
    public void ComputerTurnEnd()
    {
        playerTurn=true;
    }    


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
