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
            AddRelativeForce(Vector2.up);
        }

        private void OnDown()
        {
            AddRelativeForce(Vector2.down);
        }

        private void AddRelativeForce(Vector2 localVector)
        {
            _rigidbody2D.AddForce(transform.localRotation * localVector);
        }
    }
}