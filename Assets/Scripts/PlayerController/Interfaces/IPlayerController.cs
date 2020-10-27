using System;
using InputSystem.Interfaces;

namespace PlayerController.Interfaces
{
    public interface IPlayerController
    {
        bool IsGameOver();
        void Initialize(IInputSystem inputSystem, MapCoordinates mapCoordinates);
        void DirectUpdate();
        ITarget GetTarget();
        uint GetMaxLives();
        Action<uint> OnLivesChange { get; set; }
        void Reset();
    }
}