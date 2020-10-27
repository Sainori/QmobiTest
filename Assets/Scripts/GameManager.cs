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

    private void Awake()
    {
        _inputSystem = GetComponent<IInputSystem>();
        _playerController = GetComponent<IPlayerController>();
        _enemyController = GetComponent<IEnemyController>();

        _mapCoordinates = new MapCoordinates(mainCamera);
        _playerController.Initialize(_inputSystem, _mapCoordinates);
        _enemyController.Initialize(_mapCoordinates, _playerController.GetTarget());
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
