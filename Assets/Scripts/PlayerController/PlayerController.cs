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
            void PlayerInitialization(IPlayer player) => player.Initialize(_inputSystem);
            _playerManager = _playerManager ?? new PoolManager<IPlayer>(playerPrefab, PlayerInitialization, 1);
            SpawnPlayer();

            void BulletInitialization(Bullet bullet) => bullet.Initialize(_mapCoordinates, _currentPlayer);
            _bulletManager = _bulletManager ?? new PoolManager<Bullet>(bulletPrefab, BulletInitialization, 1);
            _teleportSystem = new TeleportSystem(_mapCoordinates, _currentPlayerTransform, teleportOffset, cornerTolerance);
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
            Debug.Log("Spawn player");
            _currentPlayer.OnFire += () =>
            {
                Debug.Log("OnFire " + _currentPlayerTransform.GetHashCode());
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