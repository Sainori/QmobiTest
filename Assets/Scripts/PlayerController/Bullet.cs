using PlayerController.Interfaces;
using PoolManager;
using UnityEngine;

namespace PlayerController
{
    public class Bullet : PoolObject
    {
        [SerializeField] private int forceMultiplier = 30;

        private MapCoordinates _mapCoordinates;
        private Rigidbody2D _rigidbody;
        private ITarget _playerTarget;

        public void Initialize(MapCoordinates mapCoordinates, ITarget playerTarget)
        {
            _mapCoordinates = mapCoordinates;
            _rigidbody = GetComponent<Rigidbody2D>();
            _playerTarget = playerTarget;
        }

        public override void Activate()
        {
            base.Activate();
            var inFrontOfPlayer = _playerTarget.GetLocalRotation() * Vector2.up;
            transform.position = _playerTarget.GetCurrentPosition() + inFrontOfPlayer;
            _rigidbody.AddForce(inFrontOfPlayer * forceMultiplier, ForceMode2D.Impulse);
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.CompareTag(gameObject.tag))
            {
                return;
            }

            Deactivate();
        }

        public override void DirectUpdate()
        {
            if (!_mapCoordinates.IsOutOfMap(transform.position))
            {
                return;
            }

            Deactivate();
        }
    }
}