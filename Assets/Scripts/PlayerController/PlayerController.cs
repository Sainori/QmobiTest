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

        public void Initialize(IInputSystem inputSystem, MapCoordinates mapCoordinates)
        {
            _mapCoordinates = mapCoordinates;
            _bulletManager = new PoolManager<Bullet>(CreateObject, 5);

            var playerGameObject = Instantiate(playerPrefab);
            var player = playerGameObject.GetComponent<IPlayer>();
            player.Initialize(inputSystem, _bulletManager);
            _teleportSystem = new TeleportSystem(_mapCoordinates, playerGameObject.transform, teleportOffset, cornerTolerance);
        }

        private Bullet CreateObject(bool isActive)
        {
            var enemyObject = Instantiate(bulletPrefab);
            var bullet = enemyObject.GetComponent<Bullet>();
            bullet.Initialize(_mapCoordinates);

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