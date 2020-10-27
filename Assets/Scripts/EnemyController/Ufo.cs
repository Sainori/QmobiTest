using PlayerController.Interfaces;
using UnityEngine;

namespace EnemyController
{
    public class Ufo : Enemy
    {
        [SerializeField] private float ufoScale = 10;
        private SpawnPointGenerator _spawnPointGenerator;
        private ITarget _target;
        private Rigidbody2D _rigidbody;

        public void Initialize(SpawnPointGenerator spawnPointGenerator, ITarget target)
        {
            _target = target;
            _spawnPointGenerator = spawnPointGenerator;
            _rigidbody = GetComponent<Rigidbody2D>();
        }

        public override void Activate()
        {
            if (!IsDead)
            {
                return;
            }

            transform.localScale = Vector3.one * ufoScale;
            var spawnPoint = _spawnPointGenerator.GetSpawnPoint();
            transform.position = spawnPoint;

            base.Activate();
        }

        public override void Deactivate()
        {
            if (IsDead)
            {
                return;
            }

            base.Deactivate();
        }

        public override void DirectUpdate()
        {
            ChaseTheTarget();
        }

        private void ChaseTheTarget()
        {
            if (_target == null)
            {
                return;
            }

            _rigidbody.velocity = (_target.GetCurrentPosition() - (Vector2) transform.position).normalized;
        }
    }
}