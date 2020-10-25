using InputSystem.Interfaces;
using PlayerController.Interfaces;
using PlayerController.Models;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;

    private MapCoordinates _mapCoordinates;
    private IInputSystem _inputSystem;
    private IPlayerController _playerController;

    private void Awake()
    {
        _inputSystem = GetComponent<IInputSystem>();
        _playerController = GetComponent<IPlayerController>();

        _mapCoordinates = new MapCoordinates(mainCamera);
        _playerController.Initialize(_inputSystem, _mapCoordinates);
    }

    private void Update()
    {
        _inputSystem.DirectUpdate();
    }

    private void FixedUpdate()
    {
        _playerController.DirectUpdate();
    }
}
