using System;
using PlayerController.Interfaces;
using PoolManager.Interfaces;
using UnityEngine;

namespace PlayerController
{
    public class Bullet : MonoBehaviour, IPoolObject
    {
        [SerializeField] private int forceMultiplier = 30;

        private MapCoordinates _mapCoordinates;
        private Rigidbody2D _rigidbody;
        private ITarget _playerTarget;
        public bool IsDead { get; private set; } = true;
        public Action OnActivate { get; set; } = () => { };
        public Action OnDeactivate { get; set; } = () => { };

        public void Initialize(MapCoordinates mapCoordinates, ITarget playerTarget)
        {
            _mapCoordinates = mapCoordinates;
            _rigidbody = GetComponent<Rigidbody2D>();
            _playerTarget = playerTarget;
        }

        public void Activate()
        {
            if (!IsDead)
            {
                return;
            }

            OnActivate();
            gameObject.SetActive(true);

            var inFrontOfPlayer = _playerTarget.GetLocalRotation() * Vector2.up;
            transform.position = _playerTarget.GetCurrentPosition() + inFrontOfPlayer;
            IsDead = false;
            _rigidbody.AddForce(inFrontOfPlayer * forceMultiplier, ForceMode2D.Impulse);
        }

        public void Deactivate(bool force = false)
        {
            if (IsDead && !force)
            {
                return;
            }

            OnDeactivate();

            transform.position = Vector3.zero;
            transform.rotation = Quaternion.identity;
            transform.localScale = Vector3.one;

            OnActivate = () => { };
            OnDeactivate = () => { };

            IsDead = true;
            gameObject.SetActive(false);
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.CompareTag(gameObject.tag))
            {
                return;
            }

            Deactivate();
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