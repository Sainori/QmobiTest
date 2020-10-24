using InputSystem.Interfaces;
using PlayerController.Interfaces;
using PlayerController.Models;
using UnityEngine;

namespace PlayerController
{
    public class PlayerController : MonoBehaviour, IPlayerController
    {
        [SerializeField] private GameObject playerPrefab;
        private TeleportSystem _teleportSystem;

        public void Initialize(IInputSystem inputSystem, MapCoordinates mapCoordinates)
        {
            var playerGameObject = Instantiate(playerPrefab);
            var player = playerGameObject.GetComponent<IPlayer>();
            player.Initialize(inputSystem);

            _teleportSystem = new TeleportSystem(mapCoordinates, playerGameObject.transform);
        }

        public void DirectUpdate()
        {
            _teleportSystem.DirectUpdate();
        }
    }
}