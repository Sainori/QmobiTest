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

        public void Initialize(MapCoordinates mapCoordinates)
        {
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
            
        }

        private bool IsNeedToSpawnEnemy()
        {
            return true;
        }
    }
}