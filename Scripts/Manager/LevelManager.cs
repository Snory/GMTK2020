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
    public CinemachineBrain MainCameraBrain;
    public CinemachineVirtualCamera PlayerCam;
    public event LevelStartedDelegate LevelStarted;
    private bool _levelStarted;


    private void Awake()
    {
        if(_instance == null)
        {
            _instance = this;
          }

    }

    // Start is called before the first frame update
    void Start()
    {
        WindObject = GameObject.FindGameObjectWithTag("Player");
        LampionObject = GameObject.FindGameObjectWithTag("Lampion");
        LampionObject.GetComponent<Lampion>().DestinationReached += WinGame;
        _levelStarted = false;
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
        this.Invoke("HideGround", 2f);
        RaiseGameStarted();

    }

    private void RaiseGameStarted()
    {
        if(LevelStarted != null)
        {
            LevelStarted();
        }
    }



    public void WinGame()
    {
        SetWind(false);
        HUDManager.Instance.WinGame();
        GameManager.Instance.UpdateState(GameState.PAUSED);
    }

    public void LoseGame()
    {
        SetWind(false);
        HUDManager.Instance.LoseGame();
        GameManager.Instance.UpdateState(GameState.PAUSED);
    }

    private void HideGround()
    {
        Ground.SetActive(false);
    }

    private void SetWind(bool isActive)
    {
        WindObject.GetComponent<WindAmbient>().WindAmbientPointer.enabled = isActive;
        WindObject.SetActive(isActive);
    }
}
