using System;

namespace PoolManager.Interfaces
{
    public interface IPoolObject
    {
        bool IsDead { get; }
        Action OnDestroy { get; set; }
        Action OnActivate { get; set; }
        Action OnDeactivate { get; set; }

        void Activate();
        void Deactivate();
        void DirectUpdate();
    }
}