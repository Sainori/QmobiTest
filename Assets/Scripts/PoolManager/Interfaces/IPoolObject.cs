using System;

namespace PoolManager.Interfaces
{
    public interface IPoolObject
    {
        bool IsDead { get; }
        Action OnActivate { get; set; }
        Action OnDeactivate { get; set; }
        void Activate();
        void Deactivate(bool force = false);
        void DirectUpdate();
    }
}