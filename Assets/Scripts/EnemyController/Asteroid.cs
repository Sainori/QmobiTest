using EnemyController.Interfaces;
using UnityEngine;

namespace EnemyController
{
    public class Asteroid : Enemy, IKillable
    {
        private const int AsteroidScale = 10;
        private bool _isJustSpawned;

        private MapCoordinates _mapCoordinates;
        private Rigidbody2D _rigidbody;
        private SpawnPointGenerator _spawnPointGenerator;
        private StartForceGenerator _startForceGenerator;

        public void Initialize(MapCoordinates mapCoordinates, SpawnPointGenerator spawnPointGenerator,
            StartForceGenerator startForceGenerator)
        {
            _rigidbody = gameObject.GetComponent<Rigidbody2D>();
            _spawnPointGenerator = spawnPointGenerator;
            _startForceGenerator = startForceGenerator;
            _mapCoordinates = mapCoordinates;
        }

        public new void Activate()
        {
            if (!IsDead)
            {
                return;
            }

            base.Activate();

            transform.localScale = Vector3.one * AsteroidScale;
            _isJustSpawned = true;
            var spawnPoint = _spawnPointGenerator.GetSpawnPoint();
            transform.position = spawnPoint;
            _rigidbody.AddForce(_startForceGenerator.GetStartForce(spawnPoint), ForceMode2D.Impulse);
        }

        public new void Deactivate()
        {
            if (IsDead)
            {
                return;
            }

            _isJustSpawned = false;
            _rigidbody.velocity = Vector2.zero;

            base.Deactivate();
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

        public void TakeDamage()
        {
            Deactivate();
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.CompareTag(gameObject.tag))
            {
                return;
            }

            other.transform.GetComponent<IKillable>()?.TakeDamage();
        }
    }
}