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

        [SerializeField] private int maxLives = 3;
        [SerializeField] private int currentLives;

        private GameObject _playerGameObject;

        private IInputSystem _inputSystem;
        private MapCoordinates _mapCoordinates;
        private TeleportSystem _teleportSystem;

        private IPoolManager<Bullet> _bulletManager;
        private IPoolManager<Player> _playerManager;
        private Player _currentPlayer;

        private float spawnDelay = 1f;
        private float timeBeforeSpawn = 0;

        public bool IsGameOver()
        {
            return currentLives <= 0;
        }

        public void Initialize(IInputSystem inputSystem, MapCoordinates mapCoordinates)
        {
            timeBeforeSpawn = spawnDelay; //because we need to spawn player immediately

            currentLives = maxLives;
            _inputSystem = inputSystem;

            _mapCoordinates = mapCoordinates;
            _playerManager = new PoolManager<Player>(CreatePlayer, 1);
            _bulletManager = new PoolManager<Bullet>(CreateBullet, 5);

            _teleportSystem = new TeleportSystem(_mapCoordinates, _playerGameObject.transform, teleportOffset, cornerTolerance);
        }

        private Player CreatePlayer(bool isActive)
        {
            _playerGameObject = Instantiate(playerPrefab);
            var player = _playerGameObject.GetComponent<Player>();
            player.OnFire += () =>
            {
                var bullet = _bulletManager.GetPoolObject();
                bullet.Activate();
            };

            player.Initialize(_inputSystem);


            player.gameObject.SetActive(false);
            // if (isActive)
            // {
                // player.Activate();
            // }
            // else
            // {
                // player.Deactivate();
            // }

            return player;
        }

        private Bullet CreateBullet(bool isActive)
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

            TrySpawnPlayer();
        }

        private void TrySpawnPlayer()
        {
            if (currentLives <= 0 || _currentPlayer != null)
            {
                return;
            }

            if (timeBeforeSpawn <= spawnDelay)
            {
                timeBeforeSpawn += Time.deltaTime;
                return;
            }

            timeBeforeSpawn = 0;

            _currentPlayer = _playerManager.GetPoolObject();
            _currentPlayer.Activate();

            _currentPlayer.OnDeactivate += () =>
            {
                _currentPlayer = null;
                currentLives--;
            };
        }
    }
}