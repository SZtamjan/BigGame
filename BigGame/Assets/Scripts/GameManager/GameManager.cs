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
    public static GameObject _this;

    [Header("Starting Hex Grid Function")]
    [SerializeField]
    public GameObject hexGrid;

    [Header("Kto zaczyna")]
    public bool playerTurn = true;
    public bool devMode = false;
    public static int turnCounter = 1;
    public GameObject turnDisplay;
    public GameObject endScreen;
    public GameObject turnButton;
    public float ileSekundCzekac = 0.75f;
    public int coIleTurAiSpawn = 5;
   




    public static event Action<GameState> onGameStateChange;

    private void Awake()
    {
        _this = gameObject;
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
                playerTurn = true;
                turnButton.GetComponent<Button>().interactable = true;
                break;
            case GameState.EnemyTurn:
                playerTurn = false;
                turnButton.GetComponent<Button>().interactable = false;
                EnemyMove();
                break;
            case GameState.Victory:
                ShowVictoryScreen();
                StopGame();
                break;
            case GameState.Lose:
                ShowLoseScreen();
                StopGame();
                break;
           
            default:
                break;
        }

        onGameStateChange?.Invoke(newState);
    }

    private void StopGame()
    {
        turnButton.GetComponent<Button>().interactable = false;
    }

    private void ShowLoseScreen()
    {
        endScreen.SetActive(true);
        endScreen.GetComponent<TextMeshProUGUI>().text = "DEFEAT";
    }

    private void ShowVictoryScreen()
    {
        endScreen.SetActive(true);
        endScreen.GetComponent<TextMeshProUGUI>().text = "VICTORY";
    }

    public void EnemyMove()
    {
        if (!devMode)
        {
            StartCoroutine(AiMoveCoroutine());

        }
        else
        {
            GameManager.instance.UpdateGameState(GameState.PlayerTurn);
        }

    }

    IEnumerator AiMoveCoroutine()
    {
        

        yield return new WaitForSeconds(ileSekundCzekac);
        if (turnCounter % coIleTurAiSpawn == 0)
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
        endScreen.SetActive(false);
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
            
            GetComponent<PatchControler>().PlayerUnitMove();

            turnCounter++;     

            GameManager.instance.UpdateGameState(GameState.EnemyTurn);

        }



    }

    public void ComputerTurnEnd()
    {

        if (CanComputerMove())
        {
            
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
