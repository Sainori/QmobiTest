using System;
using InputSystem.Interfaces;
using PlayerController.Interfaces;
using PoolManager;
using PoolManager.Interfaces;
using UnityEngine;

namespace PlayerController
{
    public class PlayerController : MonoBehaviour, IPlayerController
    {
        [SerializeField] private GameObject playerPrefab;
        [SerializeField] private GameObject bulletPrefab;
        [SerializeField] private float cornerTolerance = 3f;
        [SerializeField] private float teleportOffset = 1f;

        private TeleportSystem _teleportSystem;
        private IPoolManager<Bullet> _bulletManager;
        private MapCoordinates _mapCoordinates;
        private GameObject _playerGameObject;

        public void Initialize(IInputSystem inputSystem, MapCoordinates mapCoordinates)
        {
            _mapCoordinates = mapCoordinates;
            _playerGameObject = Instantiate(playerPrefab);
            var player = _playerGameObject.GetComponent<IPlayer>();

            _bulletManager = new PoolManager<Bullet>(CreateObject, 5);
            player.Initialize(inputSystem, _bulletManager);

            _teleportSystem = new TeleportSystem(_mapCoordinates, _playerGameObject.transform, teleportOffset, cornerTolerance);
        }

        private Bullet CreateObject(bool isActive)
        {
            var bulletObject = Instantiate(bulletPrefab);
            var bullet = bulletObject.GetComponent<Bullet>();
            bullet.Initialize(_mapCoordinates, _playerGameObject.transform);

            if (isActive)
            {
                bullet.Activate();
            }
            else
            {
                bullet.Deactivate();
            }

            return bullet;
        }

        public void DirectUpdate()
        {
            _teleportSystem.DirectUpdate();
            _bulletManager.UpdateEnabledObjects();
        }
    }
}