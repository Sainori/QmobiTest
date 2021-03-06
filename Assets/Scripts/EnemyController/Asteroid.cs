using UnityEngine;

namespace EnemyController
{
    public class Asteroid : Enemy
    {
        [SerializeField] private uint scoreReward = 10;

        private const int AsteroidScale = 10;
        private bool _isJustSpawned;

        private MapCoordinates _mapCoordinates;
        private Rigidbody2D _rigidbody;
        private SpawnPointGenerator _spawnPointGenerator;
        private StartForceGenerator _startForceGenerator;

        public override uint GetScoreReward() => scoreReward;

        public void Initialize(MapCoordinates mapCoordinates, SpawnPointGenerator spawnPointGenerator,
            StartForceGenerator startForceGenerator)
        {
            _rigidbody = gameObject.GetComponent<Rigidbody2D>();
            _spawnPointGenerator = spawnPointGenerator;
            _startForceGenerator = startForceGenerator;
            _mapCoordinates = mapCoordinates;
        }

        public override void Activate()
        {
            base.Activate();
            OnAsteroidActivate();
        }

        private void OnAsteroidActivate()
        {
            transform.localScale = Vector3.one * AsteroidScale;
            _isJustSpawned = true;
            var spawnPoint = _spawnPointGenerator.GetSpawnPoint();
            transform.position = spawnPoint;
            _rigidbody.AddForce(_startForceGenerator.GetStartForce(spawnPoint), ForceMode2D.Impulse);
        }

        public override void Deactivate(bool force = false)
        {
            base.Deactivate(force);
            _isJustSpawned = false;
            _rigidbody.velocity = Vector2.zero;
        }

        public override void DirectUpdate()
        {
            if (IsDead)
            {
                return;
            }

            TryDestroy();
        }

        private void TryDestroy()
        {
            var isOutOfMap = _mapCoordinates.IsOutOfMap(transform.position);
            if (isOutOfMap && _isJustSpawned)
            {
                return;
            }

            if (!isOutOfMap && _isJustSpawned)
            {
                _isJustSpawned = false;
                return;
            }

            if (!isOutOfMap)
            {
                return;
            }

            Deactivate();
        }
    }
}