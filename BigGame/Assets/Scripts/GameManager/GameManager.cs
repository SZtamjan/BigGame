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
    public static GameObject gameManager;
    public GameState state;

    [Header("Starting Hex Grid Function")]
    public GameObject hexGrid;

    [Header("Kto zaczyna")]
    public bool playerTurn = true;
    public bool devMode = false;
    public static int turnCounter = 1;
    public GameObject turnDisplay;
    public GameObject endScreen;
    public GameObject turnButton;
    public int coIleTurAiSpawn = 5;


    public static event Action<GameState> OnGameStateChange;

    private void Awake()
    {
        gameManager = gameObject;
        instance = this;
    }

    void Start()
    {
        turnCounter = 1;
        UpdateGameState(GameState.Start);

    }

    // Update is called once per frame


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
                GameStatePlayerTurn();
                break;
            case GameState.EnemyTurn:
                GameStateEnemyTurn();
                break;
            case GameState.Victory:
                GameStateVictory();
                break;
            case GameState.Lose:
                GameStateLose();
                break;
            default:
                break;
        }

        OnGameStateChange?.Invoke(newState);
    }


    #region  GamaState Functions

    private void StartingFunction()
    {
        endScreen.SetActive(false);

        GameManager.instance.UpdateGameState(GameState.MapGeneration);

    }

    private void GenerateHexGrid()
    {
        hexGrid.GetComponent<HexGrid>().GenerateHexGrid();

        gameObject.GetComponent<PathGenerator>().PatchGenerator();
        gameObject.GetComponent<PatchControler>().StartPath();

        GameManager.instance.UpdateGameState(GameState.PlayerTurn);

    }

    private void GameStatePlayerTurn()
    {
        UpdateTurnShower();
        playerTurn = true;
        Invoke("ActivateTurnButton", devMode ? 0f : 1f);
    }

    private void GameStateEnemyTurn()
    {
        playerTurn = false;
        
        EnemyMove();
        turnCounter++;
    }

    private void GameStateVictory()
    {
        ShowVictoryScreen();
        StopGame();
    }

    private void GameStateLose()
    {
        ShowLoseScreen();
        StopGame();
    }

    #endregion

    void DisableTurnButton()
    {
        turnButton.GetComponent<Button>().interactable = false;
    }

    void ActivateTurnButton()
    {
        turnButton.GetComponent<Button>().interactable = true;
    }


    private void StopGame()
    {
        DisableTurnButton();
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
            if (turnCounter%coIleTurAiSpawn==1)
            {
                GetComponent<SpawnerScript>().SpawnEnemyUnit();
            }
            GetComponent<PatchControler>().ComputerUnitPhaze();

        }
        else
        {
            GameManager.instance.UpdateGameState(GameState.PlayerTurn);
        }

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

    public void PlayerTurnEnd()
    {
        if (CanPlayerMove())
        {
            //DisableTurnButton();
            GetComponent<PatchControler>().PlayerUnitPhase();
        }
    }

    public void ComputerTurnEnd()
    {

        if (CanComputerMove())
        {

            GetComponent<PatchControler>().ComputerUnitPhaze();

        }


    }

    public void UpdateTurnShower()
    {
        turnDisplay.GetComponent<TextMeshProUGUI>().text = $"TURN: {turnCounter}";
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



}
