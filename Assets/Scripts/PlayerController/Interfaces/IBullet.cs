using PoolManager.Interfaces;

namespace PlayerController.Interfaces
{
    public interface IBullet : IPoolObject
    {
        void Initialize(MapCoordinates mapCoordinates, ITarget playerTarget);
    }
}