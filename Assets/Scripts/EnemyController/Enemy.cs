using EnemyController.Interfaces;
using UnityEngine;

namespace EnemyController
{
    public class Enemy : MonoBehaviour, IEnemy
    {
        private bool _justSpawned;
        private MapCoordinates _mapCoordinates;
        public bool IsDead { get; private set; }

        public void Initialize(MapCoordinates mapCoordinates)
        {
            IsDead = false;
            _justSpawned = true;
            _mapCoordinates = mapCoordinates;
        }

        public void DirectUpdate()
        {
            if (IsDead)
            {
                return;
            }

            TryDestroy();
        }


        private void TryDestroy()
        {
            var isOutOfMap = _mapCoordinates.IsOutOfMap(transform.position);
            if (isOutOfMap && _justSpawned)
            {
                return;
            }

            if (!isOutOfMap && _justSpawned)
            {
                _justSpawned = false;
                return;
            }

            if (!isOutOfMap)
            {
                return;
            }

            IsDead = true;
            Destroy(gameObject);
        }
    }
}