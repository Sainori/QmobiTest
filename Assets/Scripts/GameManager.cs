using InputSystem.Interfaces;
using PlayerController.Interfaces;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Camera _camera;

    private IInputSystem _inputSystem;
    private IPlayerController _playerController;

    private void Awake()
    {
        _inputSystem = GetComponent<IInputSystem>();
        _playerController = GetComponent<IPlayerController>();
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
