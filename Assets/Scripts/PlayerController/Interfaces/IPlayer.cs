using InputSystem.Interfaces;
using PoolManager.Interfaces;

namespace PlayerController.Interfaces
{
    public interface IPlayer
    {
        void Initialize(IInputSystem inputSystem, IPoolManager<Bullet> bulletManger);
    }
}