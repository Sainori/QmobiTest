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
        private const float SpawnDelay = 0.5f;

        public void Initialize(MapCoordinates mapCoordinates)
        {
            _time = 0f;
            _startForceGenerator = new StartForceGenerator(mapCoordinates, minStartForce, maxStartForce);
            _spawnPointGenerator = new SpawnPointGenerator(mapCoordinates, spawnPointOffset);
        }

        public void DirectUpdate()
        {
            if (!IsNeedToSpawnEnemy())
            {
                return;
            }

            SpawnEnemy();
            UpdateEnemies();
        }

        private void UpdateEnemies()
        {

        }

        private void SpawnEnemy()
        {
            var spawnPoint = _spawnPointGenerator.GetSpawnPoint();
            var enemy = Instantiate(enemyPrefab, spawnPoint, Quaternion.identity);
            enemy.GetComponent<Rigidbody2D>().AddForce(_startForceGenerator.GetStartForce(spawnPoint), ForceMode2D.Impulse);
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