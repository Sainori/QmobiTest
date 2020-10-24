using InputSystem.Interfaces;
using PlayerController.Interfaces;
using UnityEngine;

namespace PlayerController
{
    public class Player : MonoBehaviour, IPlayer
    {
        private Rigidbody2D _rigidbody2D;

        public void Initialize(IInputSystem inputSystem)
        {
            _rigidbody2D = transform.GetComponent<Rigidbody2D>();
            SetupControl(inputSystem);
        }

        private void SetupControl(IInputSystem inputSystem)
        {
            inputSystem.OnUp += OnUp;
            inputSystem.OnDown += OnDown;
            inputSystem.OnRight += OnRight;
            inputSystem.OnLeft += OnLeft;
        }

        private void ResetControl(IInputSystem inputSystem)
        {
            inputSystem.OnUp -= OnUp;
            inputSystem.OnDown -= OnDown;
            inputSystem.OnRight -= OnRight;
            inputSystem.OnLeft -= OnLeft;
        }

        private void OnLeft()
        {
            transform.Rotate(Vector3.forward);
        }

        private void OnRight()
        {
            transform.Rotate(Vector3.back);
        }

        private void OnUp()
        {
            _rigidbody2D.AddForce(transform.localRotation * Vector3.up);
        }

        private void OnDown()
        {
            _rigidbody2D.AddForce(_rigidbody2D.velocity * -0.5f);
        }
    }
}