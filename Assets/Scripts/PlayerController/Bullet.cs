using System;
using PoolManager.Interfaces;
using UnityEngine;

namespace PlayerController
{
    public class Bullet : MonoBehaviour, IPoolObject
    {
        private MapCoordinates _mapCoordinates;
        private Rigidbody2D _rigidbody;
        private Transform _playerTransform;
        public bool IsDead { get; private set; }
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
            OnActivate();
            gameObject.SetActive(true);

            var inFrontOfPlayer = _playerTransform.rotation * Vector2.up;
            transform.position = _playerTransform.position + inFrontOfPlayer;
            IsDead = false;
            _rigidbody.AddForce(inFrontOfPlayer * 10, ForceMode2D.Impulse);
        }

        public void Deactivate()
        {
            OnDeactivate();

            transform.position = Vector3.zero;
            transform.rotation = Quaternion.identity;
            transform.localScale = Vector3.one;

            OnActivate = null;
            OnDeactivate = null;

            IsDead = true;
            gameObject.SetActive(false);;
        }

        // public void SetImpulse(Vector2 force)
        // {
        //     _rigidbody.AddForce(force, ForceMode2D.Impulse);
        // }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.CompareTag(gameObject.tag))
            {
                return;
            }

            other.transform.GetComponent<IKillable>()?.TakeDamage();
        }

        public void DirectUpdate()
        {
            if (!_mapCoordinates.IsOutOfMap(transform.position))
            {
                return;
            }

            Deactivate();
        }
    }
}