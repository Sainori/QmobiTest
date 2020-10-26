using System;
using InputSystem.Interfaces;

namespace PlayerController.Interfaces
{
    public interface IPlayer
    {
        Action OnFire { get; set; }
        void Initialize(IInputSystem inputSystem);
    }
}