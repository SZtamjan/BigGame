using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
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

    public List<UnitScriptableObjects> TestDoSpawn;

    private void Awake()
    {
        gameManager = gameObject;
        instance = this;
    }

    void Start()
    {
        turnCounter = 1;

        SaveProgress();
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
                CreatePaths();
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
        CardManager.instance.SpawnStartCards();
        GameManager.instance.UpdateGameState(GameState.PlayerTurn);

    }

    private void CameraSetting()
    {
        PlayerMovement.instance.CameraSetting();
    }


    #region  GamaState Functions

    private void StartingFunction()
    {
        StartCoroutine(StartingFuctiom());
        //PathControler.Instance.StartNewPathWay();
        // GameManager.instance.UpdateGameState(GameState.MapGeneration);

    }

    private IEnumerator StartingFuctiom()
    {
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        //PathControler.Instance.StartNewPathWay();
        GameManager.instance.UpdateGameState(GameState.MapGeneration);
    }

    private void CreatePaths()
    {
        CastlesController.Instance.GatesInitialization();
    }

    private void GameStatePlayerTurn()
    {
        GetComponent<SpawnerScript>().SetRemoved(false);
        EventManager.Instance.BuldingsActions();
        if (CardManager.instance.PlayerCards.Count == 0 || turnCounter > 1)
        {
            CardManager.instance.GetNewCardToHand();
        }

        UIController.Instance.TurnButtonActivate();
        Economy.Instance.CashOnTurn();
        UpdateTurnShower();
        playerTurn = true;


    }

    public void PlayerTurnEnd()
    {
        if (CanPlayerMove())
        {

            UIController.Instance.TurnButtonDisable();

            //GetComponent<PathControler>().PlayerUnitPhase();

            //GetComponent<PathControler>().PlayerUnitPhase(); // do zrobienia
            foreach (var item in CastlesController.Instance.playerCastle.gates)
            {
                item.PlayerUnitPhase();
            }
            //CardManager.instance.WyjebReke();

            StartCoroutine(Endturn(true));
        }
    }

    private void GameStateEnemyTurn()
    {
        playerTurn = false;
        turnCounter++;
        Debug.Log("jaka� akcja przeciwnika");
        StartCoroutine(EnemyMove());
        //GameManager.instance.UpdateGameState(GameState.PlayerTurn);

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
        UIController.Instance.TurnButtonDisable();
    }

    private void ShowLoseScreen()
    {
        UIController.Instance.ShowEndDisplayActivate("DEFEAT", false);
    }

    private void ShowVictoryScreen()
    {
        UIController.Instance.ShowEndDisplayActivate("VICTORY", true);
    }

    private IEnumerator EnemyMove()
    {
        yield return new WaitForEndOfFrame();
        TestDoSpawn = new();
        for (int i = 0; i < CastlesController.Instance.enemyCastle.gates.Count; i++)
        {
            TestDoSpawn.Add(SpawnerScript.instance.WhatEnemyCanSpawn.SelectUnitAndTurnAndPath(i, turnCounter-1));
        }
        if (!devMode)
        {

            SpawnerScript.instance.EnemyCheckSpawn();

            yield return new WaitForSeconds(0.3f);
            //GetComponent<PathControler>().ComputerUnitPhaze();
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

    private IEnumerator Endturn(bool playerUnit) // do przerobienia to jest XDD
    {
        yield return new WaitForSeconds(0.3f);

        bool wait = true;
        while (wait)
        {
            wait = false;
            foreach (var item in CastlesController.Instance.playerCastle.gates)
            {
                if (ImDoingSomethingOneThisPatch(item))
                {
                    wait = true;
                    break;
                }
            }
            yield return new WaitForFixedUpdate();

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

    private bool ImDoingSomethingOneThisPatch(Gate gate)
    {
        foreach (var item in gate.path)
        {
            if (item.unitMain == null)
            {
                continue;
            }
            var thisUnit = item.unitMain.GetComponent<UnitControler>();
            if (thisUnit.AmIDoingSomething())
            {
                return true;
            }

        }

        return false;
    }

    public void UpdateTurnShower()
    {
        string turn = turnCounter.ToString();
        UIController.Instance.ShowTurnChangeNumber(turn);
    }

    public void SaveProgress()
    {
        SaveSystem saveScript = GetComponent<SaveSystem>();
        saveScript.gameData.sceneIndex = SceneManager.GetActiveScene().buildIndex;
        saveScript.SaveLevel();
    }

    public void LoadProgress()
    {
        GameData gameData = GetComponent<SaveSystem>().LoadLevel();
        SceneManager.LoadScene(gameData.sceneIndex);
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
