using System.Collections.Generic;
using EnemyController.Interfaces;
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
        private const float SpawnDelay = 3f;


        private List<IEnemy> _enemies = new List<IEnemy>();
        private MapCoordinates _mapCoordinates;

        public void Initialize(MapCoordinates mapCoordinates)
        {
            _time = 0f;
            _startForceGenerator = new StartForceGenerator(mapCoordinates, minStartForce, maxStartForce);
            _spawnPointGenerator = new SpawnPointGenerator(mapCoordinates, spawnPointOffset);
            _mapCoordinates = mapCoordinates;
        }

        public void DirectUpdate()
        {
            UpdateEnemies();
            if (!IsNeedToSpawnEnemy())
            {
                return;
            }

            SpawnEnemy();
        }

        private void UpdateEnemies()
        {
            for (var index = 0; index < _enemies.Count; index++)
            {
                var enemy = _enemies[index];
                if (enemy == null)
                {
                    _enemies.Remove(enemy);
                    index--;
                    continue;
                }

                enemy.DirectUpdate();
            }
        }

        private void SpawnEnemy()
        {
            var spawnPoint = _spawnPointGenerator.GetSpawnPoint();
            var enemyObject = Instantiate(enemyPrefab, spawnPoint, Quaternion.identity);
            enemyObject.GetComponent<Rigidbody2D>().AddForce(_startForceGenerator.GetStartForce(spawnPoint), ForceMode2D.Impulse);
            var enemy = enemyObject.GetComponent<IEnemy>();
            enemy.Initialize(_mapCoordinates);
            _enemies.Add(enemy);
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