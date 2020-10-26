using System;
using System.Collections.Generic;
using PoolManager.Interfaces;

namespace PoolManager
{
    public class PoolManager<T> : IPoolManager<T> where T : IPoolObject
    {
        private readonly Func<bool, T> _createObject;

        private List<T> _enabledObjects = new List<T>();
        private Stack<T> _disabledObjects = new Stack<T>();

        public PoolManager(Func<bool, T> createObject, int startCount)
        {
            _createObject = createObject;
            CreateStartPool(startCount);
        }

        private void CreateStartPool(int startCount)
        {
            for (int i = 0; i < startCount; i++)
            {
                _disabledObjects.Push(_createObject.Invoke(false));
            }
        }

        public T GetPoolObject()
        {
            var poolObject = _disabledObjects.Count != 0 ? _disabledObjects.Pop() : _createObject.Invoke(false);

            poolObject.OnActivate += () => OnActivate(poolObject);
            poolObject.OnDeactivate += () => OnDeactivate(poolObject);

            return poolObject;
        }

        public void UpdateEnabledObjects()
        {
            for (var index = 0; index < _enabledObjects.Count; index++)
            {
                var enemy = _enabledObjects[index];
                if (enemy == null || enemy.IsDead)
                {
                    _enabledObjects.Remove(enemy);
                    index--;
                    continue;
                }

                enemy.DirectUpdate();
            }
        }

        private void OnActivate(T poolObject)
        {
            _enabledObjects.Add(poolObject);
        }

        private void OnDeactivate(T poolObject)
        {
            _enabledObjects.Remove(poolObject);
            _disabledObjects.Push(poolObject);
        }
    }
}