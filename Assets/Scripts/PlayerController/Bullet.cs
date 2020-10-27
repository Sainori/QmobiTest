using System;
using PoolManager.Interfaces;
using UnityEngine;

namespace PlayerController
{
    public class Bullet : MonoBehaviour, IPoolObject, IKillable
    {
        [SerializeField] private int forceMultiplier = 30;

        private MapCoordinates _mapCoordinates;
        private Rigidbody2D _rigidbody;
        private Transform _playerTransform;
        public bool IsDead { get; private set; } = true;
        public Action OnActivate { get; set; } = () => { };
        public Action OnDeactivate { get; set; } = () => { };

        public void Initialize(MapCoordinates mapCoordinates, Transform playerTransform)
        {
            _mapCoordinates = mapCoordinates;
            _rigidbody = GetComponent<Rigidbody2D>();
            _playerTransform = playerTransform;
        }

        public void Activate()
        {
            if (!IsDead)
            {
                return;
            }

            OnActivate();
            gameObject.SetActive(true);

            var inFrontOfPlayer = _playerTransform.rotation * Vector2.up;
            transform.position = _playerTransform.position + inFrontOfPlayer;
            IsDead = false;
            _rigidbody.AddForce(inFrontOfPlayer * forceMultiplier, ForceMode2D.Impulse);
        }

        public void Deactivate()
        {
            if (IsDead)
            {
                return;
            }

            OnDeactivate();

            transform.position = Vector3.zero;
            transform.rotation = Quaternion.identity;
            transform.localScale = Vector3.one;

            OnActivate = null;
            OnDeactivate = null;

            IsDead = true;
            gameObject.SetActive(false);
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.CompareTag(gameObject.tag))
            {
                return;
            }

            TakeDamage();
        }

        public void DirectUpdate()
        {
            if (!_mapCoordinates.IsOutOfMap(transform.position))
            {
                return;
            }

            Deactivate();
        }

        public void TakeDamage()
        {
            Deactivate();
        }
    }
}