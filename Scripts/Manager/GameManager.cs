using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class EventGameState : UnityEvent<GameState, GameState> { };
public enum GameState { PREGAME, RUNNING, PAUSED };
public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameManager Instance { get => _instance; }
    [SerializeField]
    public string _curentLevel = string.Empty;

    [SerializeField]
    private bool _levelInstance;

    [SerializeField]
    public EventGameState GameStateChanged;



    private GameState _currentGameState = GameState.PREGAME;
    public GameState CurrentGameState { get { return _currentGameState; } private set { _currentGameState = value; } }

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(this.gameObject);
        }

    }
    

    private void CheckInput()
    {

        if (Input.GetKeyDown(KeyCode.R))
        {
            string lastLevel = _curentLevel;
            UnloadLevel(_curentLevel);
            LoadLevel(lastLevel);
            GameManager.Instance.UpdateState(GameState.RUNNING);

        }
    }

    private void Update()
    {
        CheckInput();
    }


    private void Start()
    {
        GameStateChanged = new EventGameState();
        if (!_levelInstance) { 
            Init();
        }

    }


    public void LoadLevel(string levelName)
    {
        _curentLevel = levelName;
        SceneManager.LoadScene(levelName, LoadSceneMode.Additive);


    }

    public void UnloadLevel(string levelName)
    {
        if (String.IsNullOrEmpty(levelName)) return;
        PoolManager.Instance.ReturAllFlyingItem();
        SceneManager.UnloadSceneAsync(levelName);
        _curentLevel = string.Empty;

    }



    public void UpdateState(GameState stage)
    {
        GameState _previousGameState = _currentGameState;
        _currentGameState = stage;

        switch (_currentGameState)
        {
            case GameState.PAUSED:
                Time.timeScale = 0.0f;
                break;
            case GameState.PREGAME:
                Time.timeScale = 1.0f;
                break;
            case GameState.RUNNING:
                Time.timeScale = 1.0f;
                break;

            default: break;
        }

        GameStateChanged.Invoke(_previousGameState, _currentGameState);
    }


    public void Init()
    {
        LoadLevel("Menu");

    }


    public void StartGame()
    {
        GameManager.Instance.UnloadLevel(GameManager.Instance._curentLevel);
        GameManager.Instance.LoadLevel("Level1");
        GameManager.Instance.UpdateState(GameState.RUNNING);
    }
}
