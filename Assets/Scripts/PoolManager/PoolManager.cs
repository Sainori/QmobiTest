using System;
using System.Collections.Generic;
using PoolManager.Interfaces;
using UnityEngine;
using Object = UnityEngine.Object;

namespace PoolManager
{
    public class PoolManager<T> : IPoolManager<T> where T : IPoolObject
    {
        private readonly Action<T> _initialization;
        private GameObject _prefab;

        private List<T> _enabledObjects = new List<T>();
        private Stack<T> _disabledObjects = new Stack<T>();

        public PoolManager(GameObject prefab, Action<T> initialization, int startCount)
        {
            _prefab = prefab;
            _initialization = initialization;
            CreateStartPool(startCount);
        }

        public uint GetEnabledObjectsCount() => (uint) _enabledObjects.Count;

        private void CreateStartPool(int startCount)
        {
            var difference = startCount - _disabledObjects.Count;
            if (difference <= 0)
            {
                return;
            }

            for (int i = 0; i < difference; i++)
            {
                _disabledObjects.Push(CreatePoolObject(_prefab ,_initialization));
            }
        }

        public T GetPoolObject()
        {
            var poolObject = _disabledObjects.Count != 0 ? _disabledObjects.Pop() : CreatePoolObject(_prefab, _initialization);

            poolObject.OnActivate += () => OnActivate(poolObject);
            poolObject.OnDeactivate += () => OnDeactivate(poolObject);

            return poolObject;
        }

        private T CreatePoolObject(GameObject prefab, Action<T> initialization)
        {
            var poolGameObject =  Object.Instantiate(prefab);
            var poolObject = poolGameObject.GetComponent<T>();
            initialization(poolObject);
            poolGameObject.SetActive(false);

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