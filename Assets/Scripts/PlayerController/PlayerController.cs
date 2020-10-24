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
            inputSystem.OnRight += () => transform.Rotate(Vector3.forward);
            inputSystem.OnLeft += () => transform.Rotate(Vector3.back);
        }

        public void DirectUpdate()
        {
            
        }
    }
}