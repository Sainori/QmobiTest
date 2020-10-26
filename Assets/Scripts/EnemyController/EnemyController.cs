using EnemyController.Interfaces;
using PoolManager;
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
        private SpawnPointGenerator _spawnPointGenerator;
        private StartForceGenerator _startForceGenerator;
        private float _time = 0f;
        private const float SpawnDelay = 0.3f;

        private MapCoordinates _mapCoordinates;

        private PoolManager<Asteroid> _asteroidManager;
        private PoolManager<Ufo> _ufosManager;

        public void Initialize(MapCoordinates mapCoordinates)
        {
            _time = 0f;
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
            // ufo.Initialize(_spawnPointGenerator);

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
            if (!IsNeedToSpawnEnemy())
            {
                return;
            }

            SpawnEnemy();
        }

        //TODO: add enemy spawn chance
        private void SpawnEnemy()
        {
            var asteroid = _asteroidManager.GetPoolObject();
            // var ufo = _ufosManager.GetPoolObject();

            asteroid.Activate();
            // ufo.Activate();
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