using System;
using PoolManager;
using UnityEngine;

namespace EnemyController
{
    public class Enemy : PoolObject
    {
        public Action OnKill { get; set; } = () => { };

        public override void Deactivate(bool force = false)
        {
            base.Deactivate(force);
            OnKill = () => { };
        }

        public override void DirectUpdate() { }

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