using EnemyController.Interfaces;
using InputSystem.Interfaces;
using PlayerController.Interfaces;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;

    private MapCoordinates _mapCoordinates;
    private IInputSystem _inputSystem;
    private IPlayerController _playerController;
    private IEnemyController _enemyController;
    private ScoreCounter _scoreCounter;
    private UiController.UiController _uiController;

    private void Awake()
    {
        _inputSystem = GetComponent<IInputSystem>();
        _playerController = GetComponent<IPlayerController>();
        _enemyController = GetComponent<IEnemyController>();
        _uiController = GetComponent<UiController.UiController>();

        Initialize();
    }

    private void Initialize()
    {
        _scoreCounter = new ScoreCounter();
        _mapCoordinates = new MapCoordinates(mainCamera);
        _playerController.Initialize(_inputSystem, _mapCoordinates);
        _enemyController.Initialize(_mapCoordinates, _playerController.GetTarget(), _scoreCounter);

        _inputSystem.OnRestart += OnRestart;
        _uiController.Initialize(_scoreCounter, _playerController);
    }

    private void OnRestart()
    {
        if (!_playerController.IsGameOver())
        {
            return;
        }

        Restart();
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

    private void Update()
    {
        _inputSystem.DirectUpdate();
    }

    private void FixedUpdate()
    {
        if (_playerController.IsGameOver())
        {
            Time.timeScale = 0;
            return;
        }

        _playerController.DirectUpdate();
        _enemyController.DirectUpdate();
    }
}
