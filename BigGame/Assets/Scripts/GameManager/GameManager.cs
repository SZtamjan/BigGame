using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static PathControler;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public static GameObject gameManager;
    public GameState state;

    public LayerMask layerMask;

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
        PathControler.Instance.StartNewPathWay();
        GameManager.instance.UpdateGameState(GameState.MapGeneration);

    }

    private void GenerateHexGrid()
    {
        
        PathControler.Instance.StartPath();

        GameManager.instance.UpdateGameState(GameState.PlayerTurn);

    }

    private void GameStatePlayerTurn()
    {
        ActivateTurnButton();
        Economy.Instance.CashOnTurn();
        UpdateTurnShower();
        playerTurn = true;


    }

    public void PlayerTurnEnd()
    {
        if (CanPlayerMove())
        {

            DisableTurnButton();
            GetComponent<PathControler>().PlayerUnitPhase();
            StartCoroutine(Endturn(true));
        }
    }

    private void GameStateEnemyTurn()
    {
        playerTurn = false;
        turnCounter++;
        StartCoroutine(EnemyMove());

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
        endScreen.GetComponent<SceneChange>().LoadOnClick();
    }

    private IEnumerator EnemyMove()
    {
        yield return new WaitForEndOfFrame();
        if (!devMode)
        {
            if ((turnCounter+1) % coIleTurAiSpawn == 0)
            {
                GetComponent<SpawnerScript>().SpawnEnemyUnit();
                yield return new WaitForSeconds(1f);
            }
            GetComponent<PathControler>().ComputerUnitPhaze();
            yield return new WaitForSeconds(0.3f);
            GameManager.instance.StartCoroutine(Endturn(false));

        }
        else
        {
            yield return new WaitForEndOfFrame();
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

    

    private IEnumerator Endturn(bool playerUnit)
    {
        yield return new WaitForEndOfFrame();
        bool wait = true;
        while (wait)
        {
            
            
            for (int i = 0; i <= PathWay.Count() - 1; i++)
            {

                yield return new WaitForEndOfFrame();
                if (PathWay[i].unitMain == null)
                {
                    wait = false;
                    continue;
                }
                var thisUnit = PathWay[i].unitMain;
                var thisUnitController = thisUnit.GetComponent<UnitControler>();
                if (thisUnitController.IsThisPlayerUnit() != playerUnit)
                {
                    continue;
                }
                if (thisUnitController.AmIDoingSomething())
                {
                    wait = true;
                    break;
                }
                wait = false;
            }

            GameObject UnitInCastle;
            if (playerUnit)
            {
                UnitInCastle = PathControler.Instance.PlayerCastle.jednostka;
            }
            else
            {
                UnitInCastle = PathControler.Instance.ComputerCastle.jednostka;
            }
            if (UnitInCastle != null)
            {
                if (UnitInCastle.GetComponent<UnitControler>().AmIDoingSomething())
                {
                    wait = true;
                    continue;
                }
                else
                {
                    continue;
                }
            }
            else
            {
                if (wait)
                {
                    continue;
                }
                else
                {
                    continue;
                }
            }

            
        }

        if (playerUnit)
        {
            GameManager.instance.UpdateGameState(GameState.EnemyTurn);
        }
        else
        {
            GameManager.instance.UpdateGameState(GameState.PlayerTurn);
        }
    }


    public void UpdateTurnShower()
    {
        string asd = turnCounter.ToString();
        turnDisplay.GetComponent<TextMeshProUGUI>().text = $"TURN: {asd}";
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
