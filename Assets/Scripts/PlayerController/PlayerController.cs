using InputSystem.Interfaces;
using PlayerController.Interfaces;
using UnityEngine;

namespace PlayerController
{
    public class PlayerController : MonoBehaviour, IPlayerController
    {
        [SerializeField] private GameObject playerPrefab;

        public void Initialize(IInputSystem inputSystem)
        {
            var player = Instantiate(playerPrefab);
            var rigidbody2D = player.GetComponent<Rigidbody2D>();

            inputSystem.OnUp += () => rigidbody2D.AddForce(player.transform.localRotation * Vector2.up);
            inputSystem.OnDown += () => rigidbody2D.AddForce(player.transform.localRotation * Vector2.down);
            inputSystem.OnRight += () => player.transform.Rotate(Vector3.back);
            inputSystem.OnLeft += () => player.transform.Rotate(Vector3.forward);
        }

        public void DirectUpdate()
        {
            
        }
    }
}