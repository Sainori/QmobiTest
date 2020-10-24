using InputSystem.Interfaces;
using PlayerController.Models;

namespace PlayerController.Interfaces
{
    public interface IPlayerController
    {
        void Initialize(IInputSystem inputSystem, MapCoordinates mapCoordinates);
        void DirectUpdate();
    }
}