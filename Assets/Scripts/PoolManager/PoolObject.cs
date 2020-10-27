using System;
using PoolManager.Interfaces;
using UnityEngine;

namespace PoolManager
{
    public class PoolObject : MonoBehaviour, IPoolObject
    {
        public bool IsDead { get; private set; } = true;
        public Action OnActivate { get; set; } = () => { };
        public Action OnDeactivate { get; set; } = () => { };

        public virtual void Activate()
        {
            if (!IsDead)
            {
                return;
            }

            OnActivate();

            IsDead = false;
            gameObject.SetActive(true);
        }

        public virtual void Deactivate(bool force = false)
        {
            if (IsDead && !force)
            {
                return;
            }

            OnDeactivate();

            transform.position = Vector3.zero;
            transform.rotation = Quaternion.identity;
            transform.localScale = Vector3.one;

            OnActivate = () => { };
            OnDeactivate = () => { };

            IsDead = true;
            gameObject.SetActive(false);
        }

        public virtual void DirectUpdate()
        {
        }
    }
}