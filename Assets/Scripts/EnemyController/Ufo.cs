using PlayerController.Interfaces;
using UnityEngine;

namespace EnemyController
{
    public class Ufo : Enemy
    {
        [SerializeField] private uint scoreReward = 50;
        [SerializeField] private float ufoScale = 10;
        [SerializeField] private float velocityMultiplier;
        [SerializeField] private float minVelocityMultiplier = 1f;
        [SerializeField] private float maxVelocityMultiplier = 5f;

        private SpawnPointGenerator _spawnPointGenerator;
        private ITarget _target;
        private Rigidbody2D _rigidbody;

        public override uint GetScoreReward() => scoreReward;

        public void Initialize(SpawnPointGenerator spawnPointGenerator, ITarget target)
        {
            _target = target;
            _spawnPointGenerator = spawnPointGenerator;
            _rigidbody = GetComponent<Rigidbody2D>();
        }

        public override void Activate()
        {
            base.Activate();
            OnUfoActivate();
        }

        private void OnUfoActivate()
        {
            velocityMultiplier = Random.Range(minVelocityMultiplier, maxVelocityMultiplier);
            transform.localScale = Vector3.one * ufoScale;
            transform.position = _spawnPointGenerator.GetSpawnPoint();
        }

        public override void DirectUpdate()
        {
            ChaseTheTarget();
        }

        private void ChaseTheTarget()
        {
            if (_target == null || _target.IsDead())
            {
                _rigidbody.velocity = Vector2.zero;
                return;
            }

            _rigidbody.velocity = (_target.GetCurrentPosition() - transform.position).normalized * velocityMultiplier;
        }
    }
}