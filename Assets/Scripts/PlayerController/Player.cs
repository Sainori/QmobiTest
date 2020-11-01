using System;
using InputSystem.Interfaces;
using PlayerController.Interfaces;
using PoolManager;
using UnityEngine;

namespace PlayerController
{
    public class Player : PoolObject, IPlayer
    {
        [SerializeField] private float rotationPerSecond = 180f;
        [SerializeField] private float accelerationMultiplier = 5f;
        [SerializeField] private float maxVelocityMagnitude = 10f;
        [Range(0.1f, 1f)] [SerializeField] private float breakMultiplier = 0.5f;

        private Rigidbody2D _rigidbody2D;
        private IInputSystem _inputSystem;

        public Action OnFire { get; set; } = () => { };

        public void Initialize(IInputSystem inputSystem)
        {
            _inputSystem = inputSystem;
            _rigidbody2D = transform.GetComponent<Rigidbody2D>();
        }

        public Transform GetTransform()
        {
            return transform;
        }

        private void SetupControl(IInputSystem inputSystem)
        {
            inputSystem.OnUp += OnUp;
            inputSystem.OnDown += OnDown;
            inputSystem.OnRight += OnRight;
            inputSystem.OnLeft += OnLeft;
            inputSystem.OnSpace += OnFire;
        }

        private void ResetControl(IInputSystem inputSystem)
        {
            inputSystem.OnUp -= OnUp;
            inputSystem.OnDown -= OnDown;
            inputSystem.OnRight -= OnRight;
            inputSystem.OnLeft -= OnLeft;
            inputSystem.OnSpace -= OnFire;
        }

        private void OnLeft()
        {
            transform.Rotate(Vector3.forward * rotationPerSecond * Time.deltaTime);
        }

        private void OnRight()
        {
            transform.Rotate(Vector3.back * rotationPerSecond * Time.deltaTime);
        }

        private void OnUp()
        {
            _rigidbody2D.AddForce(transform.localRotation * Vector3.up * accelerationMultiplier);
            _rigidbody2D.velocity = Vector2.ClampMagnitude(_rigidbody2D.velocity, maxVelocityMagnitude);
        }

        private void OnDown()
        {
            _rigidbody2D.AddForce(_rigidbody2D.velocity * -breakMultiplier);
        }

        public override void Activate()
        {
            base.Activate();
            SetupControl(_inputSystem);
        }

        public override void Deactivate(bool force = false)
        {
            ResetControl(_inputSystem);
            OnFire = () => { };
            base.Deactivate(force);
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.CompareTag(gameObject.tag))
            {
                return;
            }

            Deactivate();
        }

        public Vector3 GetCurrentPosition()
        {
            return transform.position;
        }

        bool ITarget.IsDead()
        {
            return IsDead;
        }

        public Quaternion GetLocalRotation()
        {
            return transform.localRotation;
        }
    }
}