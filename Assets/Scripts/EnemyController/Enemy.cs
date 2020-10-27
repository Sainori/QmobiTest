using System;
using PoolManager.Interfaces;
using UnityEngine;

namespace EnemyController
{
    public class Enemy : MonoBehaviour, IPoolObject, IKillable
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

        public virtual void Deactivate()
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

        public virtual void TakeDamage()
        {
            if (IsDead)
            {
                return;
            }

            OnKill();
            Deactivate();
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.CompareTag(gameObject.tag))
            {
                return;
            }

            TakeDamage();
        }

        public virtual uint GetScoreReward()
        {
            return 0;
        }
    }
}