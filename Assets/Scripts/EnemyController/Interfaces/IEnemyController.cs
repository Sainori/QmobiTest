using PlayerController.Interfaces;

namespace EnemyController.Interfaces
{
    public interface IEnemyController
    {
        void Initialize(MapCoordinates mapCoordinates, ITarget target);
        void DirectUpdate();
    }
}