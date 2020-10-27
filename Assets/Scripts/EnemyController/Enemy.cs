using System;
using PoolManager.Interfaces;
using UnityEngine;

namespace EnemyController
{
    public class Enemy : MonoBehaviour, IPoolObject
    {
        public bool IsDead { get; private set; } = true;
        public Action OnActivate { get; set; } = () => { };
        public Action OnDeactivate { get; set; } = () => { };

        public Action OnKill { get; set; } = () => { };

        public virtual void Activate()
        {
            OnActivate();

            IsDead = false;
            gameObject.SetActive(true);
        }

        public virtual void Deactivate(bool force = false)
        {
            OnDeactivate();

            transform.position = Vector3.zero;
            transform.rotation = Quaternion.identity;
            transform.localScale = Vector3.one;

            OnActivate = () => { };
            OnDeactivate = () => { };
            OnKill = () => { };

            IsDead = true;
            gameObject.SetActive(false);
        }

        public virtual void DirectUpdate() { }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.CompareTag(gameObject.tag))
            {
                return;
            }

            if (IsDead)
            {
                return;
            }

            OnKill();
            Deactivate();
        }

        public virtual uint GetScoreReward()
        {
            return 0;
        }
    }
}