namespace EnemyController.Interfaces
{
    public interface IEnemy
    {
        void Initialize(MapCoordinates mapCoordinates);
        void DirectUpdate();
    }
}