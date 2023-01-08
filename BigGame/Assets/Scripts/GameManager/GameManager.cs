using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Kto zaczyna")]
    [SerializeField]
    public bool playerTurn = true;
    public bool devMode = false;


    void GameManeger()
    {

    }


    public bool CanPlayerMove()
    {
        if (devMode)
        {
            return true;
        }
        if (!playerTurn)
        {
            Debug.Log("Tura komputera");
        }
        return playerTurn;
    }
    public bool CanComputerMove()
    {
        if (devMode)
        {
            return true;
        }
        if (playerTurn)
        {
            Debug.Log("Tura Gracza");
        }
        return !playerTurn;
    }

    public bool ReturnTurn()
    {        
        return playerTurn;
    }



    public void PlayerTurnEnd()
    {

        if (CanPlayerMove())
        {
            playerTurn = false;
            GetComponent<PatchControler>().PlayerUnitMove();


        }
        
        

    }
    public void ComputerTurnEnd()
    {
        if (CanComputerMove())
        {
            
            playerTurn = true;
            GetComponent<PatchControler>().ComputerUnitMove();
        }
       
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
