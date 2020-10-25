namespace EnemyController.Interfaces
{
    public interface IEnemyController
    {
        void Initialize(MapCoordinates mapCoordinates);
        void DirectUpdate();
    }
}