using EnemyController.Interfaces;
using PlayerController.Interfaces;
using PoolManager;
using UnityEngine;
using Random = UnityEngine.Random;

namespace EnemyController
{
    public class EnemyController : MonoBehaviour, IEnemyController
    {
        [SerializeField] private GameObject asteroidPrefab;
        [SerializeField] private GameObject ufoPrefab;

        [SerializeField] private float spawnPointOffset;

        [SerializeField] private float minStartForce;
        [SerializeField] private float maxStartForce;

        private float _time = 0f;
        private const float SpawnDelay = 1.5f;

        [SerializeField] private float asteroidChance = 0.7f;

        [SerializeField] private uint currentStage = 0;
        [SerializeField] private uint maxStage = 3;

        [SerializeField] private uint[] stagesMaxScore = {0, 500, 1000, 1500};
        [SerializeField] private uint maxEnemiesCount = 10;

        [SerializeField] private uint startAsteroidCount = 5;
        [SerializeField] private uint startUfoCount = 3;

        private MapCoordinates _mapCoordinates;
        private SpawnPointGenerator _spawnPointGenerator;
        private StartForceGenerator _startForceGenerator;

        private PoolManager<Ufo> _ufosManager;
        private PoolManager<Asteroid> _asteroidManager;

        private ITarget _target;
        private ScoreCounter _scoreCounter;

        public void Initialize(MapCoordinates mapCoordinates, ITarget target, ScoreCounter scoreCounter)
        {
            _time = 0f;
            currentStage = 0;
            _target = target;
            _scoreCounter = scoreCounter;

            _startForceGenerator = new StartForceGenerator(mapCoordinates, minStartForce, maxStartForce);
            _spawnPointGenerator = new SpawnPointGenerator(mapCoordinates, spawnPointOffset);
            _mapCoordinates = mapCoordinates;

            void UfoInitialization(Ufo ufo) => ufo.Initialize(_spawnPointGenerator, _target);
            void AsteroidInitialization(Asteroid asteroid) => asteroid.Initialize(_mapCoordinates, _spawnPointGenerator, _startForceGenerator);
            _ufosManager = _ufosManager ?? new PoolManager<Ufo>(ufoPrefab, UfoInitialization, startUfoCount);
            _asteroidManager = _asteroidManager ?? new PoolManager<Asteroid>(asteroidPrefab, AsteroidInitialization, startAsteroidCount);
        }

        public void DirectUpdate()
        {
            _asteroidManager.UpdateEnabledObjects();
            _ufosManager.UpdateEnabledObjects();

            TryToUpdateStage();

            var enemiesCount = _asteroidManager.GetEnabledObjectsCount() + _ufosManager.GetEnabledObjectsCount();
            if (!IsNeedToSpawnEnemy(enemiesCount))
            {
                return;
            }

            SpawnEnemy();
        }

        public void Reset()
        {
            _time = 0;
            currentStage = 0;
            _ufosManager.DeactivateAll();
            _asteroidManager.DeactivateAll();
        }

        private void TryToUpdateStage()
        {
            var score = _scoreCounter.GetScore();
            if (score < stagesMaxScore[currentStage] || currentStage == maxStage)
            {
                return;
            }

            currentStage++;
        }

        private void SpawnEnemy()
        {
            var enemy = GetEnemyForSpawn();
            enemy.Activate();
            enemy.OnKill += () => _scoreCounter.AddScore(enemy.GetScoreReward());
        }

        private Enemy GetEnemyForSpawn()
        {
            if (Random.Range(0f, 1f) <= asteroidChance)
            {
                return _asteroidManager.GetPoolObject();
            }

            return _ufosManager.GetPoolObject();
        }

        private bool IsNeedToSpawnEnemy(uint enemiesCount)
        {
            if (enemiesCount > maxEnemiesCount * currentStage)
            {
                return false;
            }

            if(_time < SpawnDelay / currentStage)
            {
                _time += Time.deltaTime;
                return false;
            }

            _time = 0f;
            return true;
        }
    }
}