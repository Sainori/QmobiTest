using EnemyController.Interfaces;
using PlayerController.Interfaces;
using PoolManager;
using PoolManager.Interfaces;
using UnityEngine;

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
            _target = target;
            _scoreCounter = scoreCounter;

            _startForceGenerator = new StartForceGenerator(mapCoordinates, minStartForce, maxStartForce);
            _spawnPointGenerator = new SpawnPointGenerator(mapCoordinates, spawnPointOffset);
            _mapCoordinates = mapCoordinates;

            _ufosManager = new PoolManager<Ufo>(CreateUfo, 3);
            _asteroidManager = new PoolManager<Asteroid>(CreateAsteroid, 1);
        }

        private Ufo CreateUfo(bool arg)
        {
            var ufoObject = Instantiate(ufoPrefab);
            var ufo = ufoObject.GetComponent<Ufo>();
            ufo.Initialize(_spawnPointGenerator, _target);

            ufoObject.SetActive(false);
            return ufo;
        }


        //TODO: it must be in another class, I think
        private Asteroid CreateAsteroid(bool isActive)
        {
            var asteroidObject = Instantiate(asteroidPrefab);
            var asteroid = asteroidObject.GetComponent<Asteroid>();
            asteroid.Initialize(_mapCoordinates, _spawnPointGenerator, _startForceGenerator);

            asteroidObject.SetActive(false);
            return asteroid;
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
            enemy.OnDeactivate += () => _scoreCounter.AddScore(enemy.GetScoreReward());
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