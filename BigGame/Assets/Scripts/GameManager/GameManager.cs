using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static PatchControler;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public GameState state;

    [Header("Starting Hex Grid Function")]
    [SerializeField]
    public GameObject hexGrid;

    [Header("Kto zaczyna")]
    public bool playerTurn = true;
    public bool devMode = false;
    public static int turnCounter = 1;
    public GameObject turnDisplay;
    public GameObject turnButton;
    public GameObject aiCompom;
    public int ileSekundCzekac = 1;




    public static event Action<GameState> onGameStateChange;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        UpdateGameState(GameState.Start);

    }

    // Update is called once per frame
    void Update()
    {
        turnDisplay.GetComponent<TextMeshProUGUI>().text = $"TURN: {turnCounter}";


    }

    public void UpdateGameState(GameState newState)
    {
        state = newState;
        switch (newState)
        {
            case GameState.Start:
                StartingFunction();
                break;
            case GameState.MapGeneration:
                GenerateHexGrid();
                break;
            case GameState.PlayerTurn:
                turnButton.GetComponent<Button>().interactable = true;
                break;
            case GameState.EnemyTurn:
                turnButton.GetComponent<Button>().interactable = false;
                EnemyMove();
                break;
            case GameState.Victory:
                break;
            case GameState.Lose:
                break;

            default:
                break;
        }

        onGameStateChange?.Invoke(newState);
    }
    public void EnemyMove()
    {
        if (!devMode)
        {
            StartCoroutine(AiMoveCoroutine());

        }

    }

    IEnumerator AiMoveCoroutine()
    {
        

        yield return new WaitForSeconds(ileSekundCzekac);
        if (turnCounter % 1 == 0)
        {
            gameObject.GetComponent<SpawnerScript>().SpawnEnemyUnit();

        }

        
        ComputerTurnEnd();
        yield return new WaitForSeconds(ileSekundCzekac);

        GameManager.instance.UpdateGameState(GameState.PlayerTurn);
    }
    private void GenerateHexGrid()
    {
        gameObject.GetComponent<PathGenerator>().PatchGenerator();
        if (playerTurn)
        {
            GameManager.instance.UpdateGameState(GameState.PlayerTurn);
        }
        else
        {
            GameManager.instance.UpdateGameState(GameState.EnemyTurn);
        }

    }

    private void StartingFunction()
    {

        hexGrid.GetComponent<HexGrid>().GenerateHexGrid();


        GameManager.instance.UpdateGameState(GameState.MapGeneration);
        gameObject.GetComponent<PatchControler>().StartPath();
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

            turnCounter++;
            //if (!devMode)
            //{
            //    turnButton.GetComponent<Button>().interactable = false;
            //}


            GameManager.instance.UpdateGameState(GameState.EnemyTurn);

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

    public enum GameState
    {
        Start,
        MapGeneration,
        PlayerTurn,
        EnemyTurn,
        Victory,
        Lose
    }

    // Start is called before the first frame update

}
