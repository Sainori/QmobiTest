namespace EnemyController
{
    public class Ufo : Enemy
    {
        // private Transform _targetTransform;
        private SpawnPointGenerator _spawnPointGenerator;

        public void Initialize(SpawnPointGenerator spawnPointGenerator)
        {
            _spawnPointGenerator = spawnPointGenerator;
            // _targetTransform = targetTransform;
        }

        public override void Activate()
        {
            // transform.localScale = Vector3.one * AsteroidScale;
            // _isJustSpawned = true;
            var spawnPoint = _spawnPointGenerator.GetSpawnPoint();
            transform.position = spawnPoint;
            // _rigidbody.AddForce(_startForceGenerator.GetStartForce(spawnPoint), ForceMode2D.Impulse);

            base.Activate();
        }

        public override void DirectUpdate()
        {
            ChaseTheTarget();
        }

        private void ChaseTheTarget()
        {
            // _rigidbody.AddForce((_target - transform.position).normalized);
        }
    }
}