using EnemyController.Interfaces;
using UnityEngine;

namespace EnemyController
{
    public class Ufo : Enemy
    {
        private Transform _targetTransform;

        public void Initialize(Transform targetTransform)
        {
            _targetTransform = targetTransform;
        }
        // public void Initialize(ITarget target)
        // {
            // _target = target;
        // }

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