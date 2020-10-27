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

        private IInputSystem _inputSystem;
        private MapCoordinates _mapCoordinates;
        private TeleportSystem _teleportSystem;

        private IPoolManager<Bullet> _bulletManager;
        private IPoolManager<IPlayer> _playerManager;

        private IPlayer _currentPlayer;
        private Transform _currentPlayerTransform;

        private float spawnDelay = 1f;
        private float _timeBeforeSpawn;

        public Action<uint> OnLivesChange { get; set; } = lives => { };

        public bool IsGameOver()
        {
            return currentLives <= 0;
        }

        public void Initialize(IInputSystem inputSystem, MapCoordinates mapCoordinates)
        {
            currentLives = maxLives;
            _inputSystem = inputSystem;

            _mapCoordinates = mapCoordinates;
            _playerManager = _playerManager ?? new PoolManager<IPlayer>(CreatePlayer, 1);
            SpawnPlayer();

            _bulletManager = _bulletManager ?? new PoolManager<Bullet>(CreateBullet, 1);
            _teleportSystem = new TeleportSystem(_mapCoordinates, _currentPlayerTransform, teleportOffset, cornerTolerance);
        }

        private IPlayer CreatePlayer(bool isActive)
        {
            var playerGameObject = Instantiate(playerPrefab);
            var player = playerGameObject.GetComponent<Player>();
            player.Initialize(_inputSystem);
            player.gameObject.SetActive(false);

            return player;
        }

        private Bullet CreateBullet(bool isActive)
        {
            var bulletObject = Instantiate(bulletPrefab);
            var bullet = bulletObject.GetComponent<Bullet>();
            bullet.Initialize(_mapCoordinates, _currentPlayer);
            bullet.gameObject.SetActive(false);

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

            if (_timeBeforeSpawn <= spawnDelay)
            {
                _timeBeforeSpawn += Time.deltaTime;
                return;
            }

            _timeBeforeSpawn = 0;
            SpawnPlayer();
        }

        private void SpawnPlayer()
        {
            _currentPlayer = _playerManager.GetPoolObject();
            _currentPlayerTransform = _currentPlayer.GetTransform();
            _currentPlayer.OnFire += () =>
            {
                Debug.Log("On Fire");
                var bullet = _bulletManager.GetPoolObject();
                bullet.Activate();
            };

            _currentPlayer.OnDeactivate += () =>
            {
                _currentPlayer = null;
                currentLives--;
                OnLivesChange((uint) currentLives);
            };

            _currentPlayer.Activate();
        }

        public ITarget GetTarget()
        {
            return _currentPlayer;
        }

        public uint GetMaxLives()
        {
            return (uint) maxLives;
        }

        public void Reset()
        {
            _timeBeforeSpawn = 0;
            _bulletManager.DeactivateAll();
            _playerManager.DeactivateAll();
        }
    }
}