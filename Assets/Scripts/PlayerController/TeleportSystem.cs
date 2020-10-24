using PlayerController.Models;
using UnityEngine;

namespace PlayerController
{
    public class TeleportSystem
    {
        private Transform _targetTransform;
        private MapCoordinates _mapCoordinates;
        private bool isNearRight;
        private bool isNearLeft;
        private bool isNearUp;
        private bool isNearDown;

        public TeleportSystem(MapCoordinates mapCoordinates, Transform targetTransform)
        {
            _mapCoordinates = mapCoordinates;
            _targetTransform = targetTransform;
        }

        public void DirectUpdate()
        {
            var isOutOfMap = IsOutOfMap(_targetTransform.position);
            if (!isOutOfMap)
            {
                return;
            }

            TeleportPlayer();
        }

        private bool IsOutOfMap(Vector3 position)
        {
            if (position.x > _mapCoordinates.RightSideBorder || position.x < _mapCoordinates.LeftSideBorder)
            {
                return true;
            }

            if (position.y > _mapCoordinates.UpSideBorder || position.y < _mapCoordinates.DownSideBorder)
            {
                return true;
            }

            return false;
        }

        private void TeleportPlayer()
        {
            isNearRight = _targetTransform.position.x >= _mapCoordinates.RightSideBorder;
            isNearLeft = _targetTransform.position.x <= _mapCoordinates.LeftSideBorder;
            isNearUp = _targetTransform.position.y >= _mapCoordinates.UpSideBorder;
            isNearDown = _targetTransform.position.y <= _mapCoordinates.DownSideBorder;

            if (!(isNearDown || isNearUp || isNearLeft || isNearRight))
            {
                return;
            }

            // if (IsNearCorner())
            // {
            // CornerTeleport();
            // }

            SideTeleport();
        }

        private void SideTeleport()
        {
            var newPosition = _targetTransform.position;
            if (isNearRight)
            {
                newPosition.x = _mapCoordinates.LeftSideBorder + 1;
            }

            if (isNearLeft)
            {
                newPosition.x = _mapCoordinates.RightSideBorder - 1;
            }

            if (isNearUp)
            {
                newPosition.y = _mapCoordinates.DownSideBorder + 1;
            }

            if (isNearDown)
            {
                newPosition.y = _mapCoordinates.UpSideBorder - 1;
            }

            _targetTransform.position = newPosition;
        }
    }
}