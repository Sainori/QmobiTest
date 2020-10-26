using System;
using PoolManager.Interfaces;
using UnityEngine;

namespace PlayerController
{
    public class Bullet : MonoBehaviour, IPoolObject
    {
        private MapCoordinates _mapCoordinates;
        private Rigidbody2D _rigidbody;
        public bool IsDead { get; private set; }
        public Action OnActivate { get; set; } = () => { };
        public Action OnDeactivate { get; set; } = () => { };

        public void Initialize(MapCoordinates mapCoordinates)
        {
            _mapCoordinates = mapCoordinates;
            _rigidbody = GetComponent<Rigidbody2D>();
        }

        public void Activate()
        {
            OnActivate();

            _rigidbody.AddForce(Vector2.up, ForceMode2D.Impulse);
            IsDead = false;
            gameObject.SetActive(true);
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