using InputSystem.Interfaces;
using PlayerController.Interfaces;
using PlayerController.Models;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Camera _camera;

    private MapCoordinates MapCoordinates;
    private IInputSystem _inputSystem;
    private IPlayerController _playerController;

    private void Awake()
    {
        _inputSystem = GetComponent<IInputSystem>();
        _playerController = GetComponent<IPlayerController>();


        SetupMapCoordinates(_camera);
        _playerController.Initialize(_inputSystem, MapCoordinates);
    }

    private void SetupMapCoordinates(Camera camera)
    {
        var upRightCorner = camera.ViewportToWorldPoint(new Vector3(1, 1));
        var downLeftCorner = camera.ViewportToWorldPoint(new Vector3(0, 0));

        MapCoordinates = new MapCoordinates(upRightCorner, downLeftCorner);
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
