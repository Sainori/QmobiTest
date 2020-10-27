using System;
using EnemyController.Interfaces;
using InputSystem.Interfaces;
using PlayerController.Interfaces;
using UnityEditor;
using UnityEngine;
using Screen = UiController.Screen;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    [SerializeField] private uint secondsToQuit = 3;

    private float _timeBeforeQuit; 

    private MapCoordinates _mapCoordinates;
    private IInputSystem _inputSystem;
    private IPlayerController _playerController;
    private IEnemyController _enemyController;
    private ScoreCounter _scoreCounter;
    private UiController.UiController _uiController;
    private GameStatus _status;

    private void Awake()
    {
        _inputSystem = GetComponent<IInputSystem>();
        _playerController = GetComponent<IPlayerController>();
        _enemyController = GetComponent<IEnemyController>();
        _uiController = GetComponent<UiController.UiController>();

        _inputSystem.OnEscape = OnEscape;
        _inputSystem.OnEscapeUp = () => _timeBeforeQuit = 0;
        _status = GameStatus.Start;
    }

    private void OnEscape()
    {
        if (_timeBeforeQuit < secondsToQuit)
        {
            _timeBeforeQuit += Time.deltaTime;
            return;
        }

        QuitGame();
    }

    private void QuitGame()
    {
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
        return;
#endif

        if (Application.platform == RuntimePlatform.WebGLPlayer)
        {
            OnRestart();
            return;
        }

        Application.Quit();
    }

    private void Initialize()
    {
        _scoreCounter = new ScoreCounter();
        _mapCoordinates = new MapCoordinates(mainCamera);
        _playerController.Initialize(_inputSystem, _mapCoordinates);
        _enemyController.Initialize(_mapCoordinates, _playerController.GetTarget(), _scoreCounter);
        _uiController.Initialize(_scoreCounter, _playerController, secondsToQuit);
    }

    private void Update()
    {
        _inputSystem.DirectUpdate();
    }

    private void OnRestart()
    {
        _uiController.SetScreen(Screen.End, false);
        _status = GameStatus.Loop;
        Restart();
    }

    private void FixedUpdate()
    {
        switch (_status)
        {
            case GameStatus.Start:
                GameStart();
                break;
            case GameStatus.Loop:
                GameLoop();
                break;
            case GameStatus.End:
                GameEnd();
                break;
        }
    }

    private void GameStart()
    {
        Time.timeScale = 0;
        _uiController.SetScreen(Screen.Start, true);
        _inputSystem.OnSpace += () =>
        {
            _uiController.SetScreen(Screen.Start, false);
            _inputSystem.OnSpace = () => { };
            _status = GameStatus.Loop;

            Initialize();

            Time.timeScale = 1;
        };

    }

    private void GameLoop()
    {
        if (_playerController.IsGameOver())
        {
            _status = GameStatus.End;
            return;
        }

        _playerController.DirectUpdate();
        _enemyController.DirectUpdate();
    }

    private void GameEnd()
    {
        _uiController.SetScreen(Screen.End, true);
        _inputSystem.OnRestart += OnRestart;
        Time.timeScale = 0;
    }

    private void Restart()
    {
        _inputSystem.OnRestart -= OnRestart;

        _inputSystem.Reset();
        _scoreCounter.ResetScore();
        _playerController.Reset();
        _enemyController.Reset();
        _uiController.Reset();

        Initialize();

        Time.timeScale = 1;
    }

    private enum GameStatus
    {
        Start,
        Loop,
        End
    }
}
