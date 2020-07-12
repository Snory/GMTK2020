using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public delegate void LevelStartedDelegate();

public class LevelManager : MonoBehaviour
{

    private static LevelManager _instance;
    public static LevelManager Instance { get => _instance; }
    public GameObject WindObject;
    public GameObject LampionObject;
    public GameObject Ground;
    public GameObject Girl;
    public CinemachineBrain MainCameraBrain;
    public CinemachineVirtualCamera PlayerCam;
    public event LevelStartedDelegate LevelStarted;
    private bool _levelStarted;
    private bool _gameWon, _gameLost;



    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }

    }

    // Start is called before the first frame update
    void Start()
    {
        WindObject = GameObject.FindGameObjectWithTag(MyTags.WIND_TAG);
        LampionObject = GameObject.FindGameObjectWithTag(MyTags.LAMPION_TAG);
        Girl = GameObject.FindGameObjectWithTag(MyTags.GIRL_TAG);
        LampionObject.GetComponent<Lampion>().DestinationReached += WinGame;
        _levelStarted = false;
        _gameWon = false;
        SetWind(false);
    }

    // Update is called once per frame
    void Update()
    {

        if (!_levelStarted)
        {
            if (MainCameraBrain.IsLive(PlayerCam))
            {
                StartGame();
            }
        }

    }

    private void StartGame()
    {

        _levelStarted = true;
        SetWind(true);
        RaiseGameStarted();
        this.Invoke("HideAfterStart", 4f);


    }

    private void RaiseGameStarted()
    {
        if (LevelStarted != null)
        {
            LevelStarted();
        }
    }



    public void WinGame()
    {
        if (!_gameLost)
        {
            SetWind(false);
            HUDManager.Instance.WinGame();
            GameManager.Instance.UpdateState(GameState.PAUSED);
            _gameWon = true;
        }
    }

    public void LoseGame()
    {
        if (!_gameWon)
        {
            SetWind(false);
            HUDManager.Instance.LoseGame();
            GameManager.Instance.UpdateState(GameState.PAUSED);
        }

    }

    private void HideAfterStart()
    {
        Ground.SetActive(false);
        Girl.SetActive(false);
    }

    private void SetWind(bool isActive)
    {
        WindObject.SetActive(isActive);
        //WindObject.GetComponent<WindAmbient>().WindAmbientPointer.enabled = isActive;

        if (isActive)
        {
            WindObject.GetComponent<WindAmbient>().EnableWind();
        }
        else
        {
            WindObject.GetComponent<WindAmbient>().DisableWind();
        }
    }
}
