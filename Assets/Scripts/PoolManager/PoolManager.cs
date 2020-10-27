using System;
using System.Collections.Generic;
using PoolManager.Interfaces;
using UnityEngine;

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

        public uint GetEnabledObjectsCount() => (uint) _enabledObjects.Count;

        private void CreateStartPool(int startCount, bool startState = false)
        {
            var difference = startCount - _disabledObjects.Count;
            if (difference <= 0)
            {
                return;
            }

            for (int i = 0; i < difference; i++)
            {
                _disabledObjects.Push(_createObject.Invoke(startState));
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
                var poolObject = _enabledObjects[index];
                if (poolObject == null || poolObject.IsDead)
                {
                    _enabledObjects.Remove(poolObject);
                    index--;
                    continue;
                }

                poolObject.DirectUpdate();
            }
        }

        public void DeactivateAll()
        {
            for (var index = 0; index < _enabledObjects.Count; index++)
            {
                var poolObject = _enabledObjects[index];
                poolObject.Deactivate(true);
                _enabledObjects.Remove(poolObject);
                index--;
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