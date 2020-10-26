using System;
using PoolManager.Interfaces;
using UnityEngine;

namespace EnemyController.Interfaces
{
    public class Enemy : MonoBehaviour, IPoolObject
    {
        public bool IsDead { get; private set; } = true;
        public Action OnActivate { get; set; } = () => { };
        public Action OnDeactivate { get; set; } = () => { };


        public void Activate()
        {
            OnActivate();

            IsDead = false;
            gameObject.SetActive(true);
        }

        public void Deactivate()
        {
            OnDeactivate();

            transform.position = Vector3.zero;
            transform.rotation = Quaternion.identity;
            transform.localScale = Vector3.one;

            OnActivate = null;
            OnDeactivate = null;

            IsDead = true;
            gameObject.SetActive(false);
        }

        public virtual void DirectUpdate() { }
    }
}