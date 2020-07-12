using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class EventGameState : UnityEvent<GameState, GameState> { };
public enum GameState { PREGAME, RUNNING, PAUSED, ACTION };
public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameManager Instance { get => _instance; }
    [SerializeField]
    private string _currentLevelName = string.Empty;

    [SerializeField]
    public EventGameState GameStateChanged;

    private List<AsyncOperation> _loadOperation;


    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(this);
        }
    }

    private GameState _currentGameState = GameState.PREGAME;


    public GameState CurrentGameState { get { return _currentGameState; } private set { _currentGameState = value; } }


    private void CheckInput()
    {

        if (Input.GetKeyDown(KeyCode.R))
        {
            LoadLevel(_currentLevelName);

        }
    }

    private void Update()
    {
        CheckInput();
    }


    private void Start()
    {
        GameStateChanged = new EventGameState();
        _loadOperation = new List<AsyncOperation>();
        Init();

    }


    public void LoadLevel(string levelName)
    {
        Debug.Log(_currentLevelName);
        Debug.Log(levelName);
        if (!String.IsNullOrEmpty(_currentLevelName)) UnloadLevel(_currentLevelName);
        SceneManager.LoadSceneAsync(levelName, LoadSceneMode.Additive);
        _currentLevelName = levelName;

    }

    public void UnloadLevel(string levelName)
    {

         SceneManager.UnloadSceneAsync(levelName);
        _currentLevelName = String.Empty;
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
        Debug.Log(_currentLevelName);
      
        LoadLevel("Level1");
        Debug.Log(_currentLevelName);
        
    }
}
