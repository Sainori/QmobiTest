using EnemyController.Interfaces;
using UnityEngine;

namespace EnemyController
{
    public class EnemyController : MonoBehaviour, IEnemyController
    {
        [SerializeField] private GameObject enemyPrefab;
        [SerializeField] private float spawnPointOffset;
        [SerializeField] private Vector3 minStartForce;
        [SerializeField] private Vector3 maxStartForce;
        private SpawnPointGenerator _spawnPointGenerator;

        public void Initialize(MapCoordinates mapCoordinates)
        {
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
            //TODO: implement spawn
        }

        private bool IsNeedToSpawnEnemy()
        {
            return true;
        }
    }
}