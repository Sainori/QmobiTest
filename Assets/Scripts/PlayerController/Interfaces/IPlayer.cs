using System;
using InputSystem.Interfaces;
using PoolManager.Interfaces;
using UnityEngine;

namespace PlayerController.Interfaces
{
    public interface IPlayer : IPoolObject, ITarget
    {
        Action OnFire { get; set; }
        void Initialize(IInputSystem inputSystem);
        Transform GetTransform();
    }
}