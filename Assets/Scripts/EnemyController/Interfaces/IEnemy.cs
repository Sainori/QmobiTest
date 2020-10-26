using PoolManager.Interfaces;

namespace EnemyController.Interfaces
{
    public interface IEnemy : IPoolObject
    {
        void Initialize(MapCoordinates mapCoordinates, SpawnPointGenerator spawnPointGenerator,
            StartForceGenerator startForceGenerator);
    }
}