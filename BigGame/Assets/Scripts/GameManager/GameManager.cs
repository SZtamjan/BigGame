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
    

   
   
    


    public static event Action<GameState> OnGameStateChange;

    private bool GameEnded = false;


    private void Awake()
    {
        gameManager = gameObject;
        instance = this;
    }

    void Start()
    {
        turnCounter = 1;
        SaveSystemTrigger saveLevelScript = GetComponent<SaveSystemTrigger>();
        if (saveLevelScript!=null)
        {
            saveLevelScript.SaveLevel();
        }
        UpdateGameState(GameState.Start);
    }

    // Update is called once per frame


    public void UpdateGameState(GameState newState)
    {
        if (!GameEnded)
        {
            state = newState;
        }
        else
        {
            state = GameState.GameEnd;
        }
        state = newState;
        switch (newState)
        {
            case GameState.Start:
                StartingFunction();
                break;
            case GameState.MapGeneration:
                GenerateHexGrid();
                CameraSetting();
                CardStart();
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

    private void CardStart()
    {
        CardManager.instance.StartSpawnCards();
    }

    private void CameraSetting()
    {
        PlayerMovement.instance.CameraSetting();
    }


    #region  GamaState Functions

    private void StartingFunction()
    {
       
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
        UIController.instance.TurnButtonActivate();
        Economy.Instance.CashOnTurn();
        UpdateTurnShower();
        playerTurn = true;


    }

    public void PlayerTurnEnd()
    {
        if (CanPlayerMove())
        {

            UIController.instance.TurnButtonDisable();
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


    private void StopGame()
    {
        UIController.instance.TurnButtonDisable();
    }

    private void ShowLoseScreen()
    {
        UIController.instance.ShowEndDisplayActivate("DEFEAT",false);       
    }

    private void ShowVictoryScreen()
    {
        UIController.instance.ShowEndDisplayActivate("VICTORY", true);        
    }

    private IEnumerator EnemyMove()
    {
        yield return new WaitForEndOfFrame();
        if (!devMode)
        {

            SpawnerScript.instance.EnemyCheckSpawn();

            yield return new WaitForSeconds(0.3f);
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
        yield return new WaitForSeconds(0.3f);
        bool wait = true;
        while (wait)
        {
            wait=false;


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
                    break; 
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
        string turn = turnCounter.ToString();
        UIController.instance.ShowTurnChangeNumber(turn);        
    }

    public enum GameState
    {
        Start,
        MapGeneration,
        PlayerTurn,
        EnemyTurn,
        Victory,
        Lose,
        GameEnd

    }



}
