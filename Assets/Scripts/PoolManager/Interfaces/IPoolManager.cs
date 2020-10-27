namespace PoolManager.Interfaces
{
    public interface IPoolManager<T> where T : IPoolObject
    {
        T GetPoolObject();
        void UpdateEnabledObjects();
        void DeactivateAll();
    }
}