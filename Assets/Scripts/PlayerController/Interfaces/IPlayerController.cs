using InputSystem.Interfaces;

namespace PlayerController.Interfaces
{
    public interface IPlayerController
    {
        void Initialize(IInputSystem inputSystem);
        void DirectUpdate();
    }
}