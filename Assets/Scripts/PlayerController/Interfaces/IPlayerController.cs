using InputSystem.Interfaces;

namespace PlayerController.Interfaces
{
    public interface IPlayerController
    {
        bool IsGameOver();
        void Initialize(IInputSystem inputSystem, MapCoordinates mapCoordinates);
        void DirectUpdate();
    }
}