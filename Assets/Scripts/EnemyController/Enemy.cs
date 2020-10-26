using System;
using EnemyController.Interfaces;
using UnityEngine;

namespace EnemyController
{
    public class Enemy : MonoBehaviour, IEnemy
    {
        private bool _justSpawned;
        private MapCoordinates _mapCoordinates;
        private Rigidbody2D _rigidbody;
        private SpawnPointGenerator _spawnPointGenerator;
        private StartForceGenerator _startForceGenerator;

        public bool IsDead { get; private set; }
        public Action OnActivate { get; set; } = () => { };
        public Action OnDeactivate { get; set; } = () => { };

        public void Initialize(MapCoordinates mapCoordinates, SpawnPointGenerator spawnPointGenerator,
            StartForceGenerator startForceGenerator)
        {
            _rigidbody = gameObject.GetComponent<Rigidbody2D>();
            _spawnPointGenerator = spawnPointGenerator;
            _startForceGenerator = startForceGenerator;
            _mapCoordinates = mapCoordinates;
        }

        public void Activate()
        {
            OnActivate();
            gameObject.SetActive(true);

            IsDead = false;
            _justSpawned = true;

            var spawnPoint = _spawnPointGenerator.GetSpawnPoint();
            transform.position = spawnPoint;
            _rigidbody.AddForce(_startForceGenerator.GetStartForce(spawnPoint), ForceMode2D.Impulse);
        }

        public void Deactivate()
        {
            OnDeactivate();
            transform.position = Vector3.zero;
            _rigidbody.velocity = Vector2.zero;

            OnActivate = null;
            OnDeactivate = null;

            gameObject.SetActive(false);
        }

        public void DirectUpdate()
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
            if (isOutOfMap && _justSpawned)
            {
                return;
            }

            if (!isOutOfMap && _justSpawned)
            {
                _justSpawned = false;
                return;
            }

            if (!isOutOfMap)
            {
                return;
            }

            IsDead = true;
            Deactivate();
        }
    }
}