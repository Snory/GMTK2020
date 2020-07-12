using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour
{

    [SerializeField]
    TextMeshProUGUI _win, _lose;


    private static HUDManager _instance;
    public static HUDManager Instance { get => _instance; }


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
        _win.enabled = false;
        LevelManager.Instance.LevelStarted += StartGame;
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void StartGame()
    {

    }

    public void WinGame()
    {
        _win.enabled = true;
    }

    public void LoseGame()
    {
        _lose.enabled = true;
    }
}
