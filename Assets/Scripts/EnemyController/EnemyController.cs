using System.Collections.Generic;
using EnemyController.Interfaces;
using PoolManager;
using UnityEngine;

namespace EnemyController
{
    public class EnemyController : MonoBehaviour, IEnemyController
    {
        [SerializeField] private GameObject enemyPrefab;
        [SerializeField] private float spawnPointOffset;

        [SerializeField] private float minStartForce;
        [SerializeField] private float maxStartForce;
        private SpawnPointGenerator _spawnPointGenerator;
        private StartForceGenerator _startForceGenerator;
        private float _time = 0f;
        private const float SpawnDelay = 0.5f;

        private List<Asteroid> _enemies = new List<Asteroid>();
        private MapCoordinates _mapCoordinates;
        private PoolManager<Asteroid> _poolManager;

        public void Initialize(MapCoordinates mapCoordinates)
        {
            _time = 0f;
            _startForceGenerator = new StartForceGenerator(mapCoordinates, minStartForce, maxStartForce);
            _spawnPointGenerator = new SpawnPointGenerator(mapCoordinates, spawnPointOffset);
            _mapCoordinates = mapCoordinates;
            _poolManager = new PoolManager<Asteroid>(CreateObject, 5);
        }
        

        //TODO: it must be in another class, I think
        private Asteroid CreateObject(bool isActive)
        {
            var enemyObject = Instantiate(enemyPrefab);
            var enemy = enemyObject.GetComponent<Asteroid>();
            enemy.Initialize(_mapCoordinates, _spawnPointGenerator, _startForceGenerator);

            if (isActive)
            {
                enemy.Activate();
            }
            else
            {
                enemy.Deactivate();
            }

            return enemy;
        }

        public void DirectUpdate()
        {
            _poolManager.UpdateEnabledObjects();
            if (!IsNeedToSpawnEnemy())
            {
                return;
            }

            SpawnEnemy();
        }

        //TODO: think about creation with 'TRUE'
        private void SpawnEnemy()
        {
            var enemy = _poolManager.GetPoolObject();
            enemy.Activate();
        }

        private bool IsNeedToSpawnEnemy()
        {
            if(_time < SpawnDelay)
            {
                _time += Time.deltaTime;
                return false;
            }

            _time = 0f;
            return true;
        }
    }
}