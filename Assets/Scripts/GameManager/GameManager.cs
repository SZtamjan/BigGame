using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static PathControler;

[SelectionBase]

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
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

    //Components
    private UnitSpawner _unitSpawner;
    
    [NonSerialized]
    public float GateTransparency = 0.33f;

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        turnCounter = 1;
        _unitSpawner = UnitSpawner.instance;

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
        //state = newState;
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
        GameManager.Instance.UpdateGameState(GameState.PlayerTurn);

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
        GameManager.Instance.UpdateGameState(GameState.MapGeneration);
    }

    private void CreatePaths()
    {
        CastlesController.Instance.GatesInitialization();
    }

    private void GameStatePlayerTurn()
    {
        _unitSpawner.SetRemoved(false); // ?????????????????????
        EventManager.Instance.NewPlayerTurnFunc();
        if (CardManager.instance.PlayerCards.Count == 0 || turnCounter > 1)
        {
            CardManager.instance.GetNewCardToHand();
        }

        UIController.Instance.TurnButtonActivate();
        EconomyResources.Instance.CashOnTurn();
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

        //tu spawn enemy jednostek
        var enemyGates = CastlesController.Instance.enemyCastle.gates;
        for (int i = 0; i < enemyGates.Count; i++)
        {
            //TestDoSpawn.Add(SpawnerScript.instance.WhatEnemyCanSpawn.SelectUnitAndTurnAndPath(i, turnCounter - 1));
            var unitToSpawn = UnitSpawner.instance.WhatEnemyCanSpawn.SelectUnitAndTurnAndPath(i, turnCounter - 1);
            if (unitToSpawn==null)
            {
                continue;
            }
            UnitSpawner.instance.SpawnEnemyUnit(CastlesController.Instance.enemyCastle.gates[i], unitToSpawn);
            enemyGates[i].SetTransparent(GateTransparency);
            yield return new WaitForEndOfFrame();
        }
        yield return new WaitForSeconds(1f);

        //a tu ich ruch
        if (!devMode)
        {

            //SpawnerScript.instance.EnemyCheckSpawn();

            yield return new WaitForEndOfFrame();
            //GetComponent<PathControler>().ComputerUnitPhaze(); stara metoda

            foreach (var item in CastlesController.Instance.playerCastle.gates)
            {
                item.EnemyUnitPhase();
            }
            yield return new WaitForEndOfFrame();


            GameManager.Instance.StartCoroutine(Endturn(false));

        }
        else
        {
            yield return new WaitForEndOfFrame();
            GameManager.Instance.UpdateGameState(GameState.PlayerTurn);
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
                    yield return new WaitForSeconds(0.5f);
                    break;
                }
            }
            yield return new WaitForFixedUpdate();

        }

        CastlesController.Instance.playerCastle.ClearPathFromWanwingUnit();

        if (playerUnit)
        {
            GameManager.Instance.UpdateGameState(GameState.EnemyTurn);
        }
        else
        {
            GameManager.Instance.UpdateGameState(GameState.PlayerTurn);
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
