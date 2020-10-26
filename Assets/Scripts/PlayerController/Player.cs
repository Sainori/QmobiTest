using InputSystem.Interfaces;
using PlayerController.Interfaces;
using PoolManager.Interfaces;
using UnityEngine;

namespace PlayerController
{
    public class Player : MonoBehaviour, IPlayer
    {
        private Rigidbody2D _rigidbody2D;
        [SerializeField] private float accelerationMultiplier = 1f;
        [Range(0.1f, 1f)] [SerializeField] private float breakMultiplier = 0.5f;

        private IPoolManager<Bullet> _bulletManager;

        public void Initialize(IInputSystem inputSystem, IPoolManager<Bullet> bulletManger)
        {
            _bulletManager = bulletManger;
            _rigidbody2D = transform.GetComponent<Rigidbody2D>();
            SetupControl(inputSystem);
        }

        public Vector2 GetCurrentPosition()
        {
            return transform.position;
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
            transform.Rotate(Vector3.forward * accelerationMultiplier);
        }

        private void OnRight()
        {
            transform.Rotate(Vector3.back * accelerationMultiplier);
        }

        private void OnUp()
        {
            _rigidbody2D.AddForce(transform.localRotation * Vector3.up * accelerationMultiplier);
        }

        private void OnDown()
        {
            _rigidbody2D.AddForce(_rigidbody2D.velocity * -breakMultiplier);
        }

        private void OnFire()
        {
            var bullet = _bulletManager.GetPoolObject();
            bullet.Activate();
            // bullet.SetImpulse(transform.localRotation * Vector3.up * accelerationMultiplier);
        }
    }
}