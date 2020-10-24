using System;
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
            var player = Instantiate(playerPrefab).GetComponent<IPlayer>();
            player.Initialize(inputSystem);
        }

        public void DirectUpdate()
        {
            
        }
    }
}