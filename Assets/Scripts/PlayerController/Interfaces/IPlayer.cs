using InputSystem.Interfaces;
using PoolManager.Interfaces;
using UnityEngine;

namespace PlayerController.Interfaces
{
    public interface IPlayer
    {
        void Initialize(IInputSystem inputSystem, IPoolManager<Bullet> bulletManger);
        Vector2 GetCurrentPosition();
    }
}