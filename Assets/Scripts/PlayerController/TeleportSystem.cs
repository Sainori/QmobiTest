using UnityEngine;

namespace PlayerController
{
    public class TeleportSystem
    {
        private readonly Transform _targetTransform;
        private readonly MapCoordinates _mapCoordinates;
        private readonly float _teleportOffset;
        private readonly float _cornerTolerance;

        private bool _isNearOrMoreRight;
        private bool _isNearOrMoreLeft;
        private bool _isNearOrMoreUp;
        private bool _isNearOrMoreDown;

        public TeleportSystem(MapCoordinates mapCoordinates, Transform targetTransform, float teleportOffset = 1f,
            float cornerTolerance = 3f)
        {
            _mapCoordinates = mapCoordinates;
            _targetTransform = targetTransform;
            _teleportOffset = teleportOffset;
            _cornerTolerance = cornerTolerance;
        }

        public void DirectUpdate()
        {
            if (!_mapCoordinates.IsOutOfMap(_targetTransform.position))
            {
                return;
            }

            TeleportPlayer();
        }

        private void TeleportPlayer()
        {
            if (TryCornerTeleport())
            {
                return;
            }

            UpdateNearFlags();
            ChangeTargetTransform();
        }

        private void UpdateNearFlags(float tolerance = 0f)
        {
            _isNearOrMoreRight = _targetTransform.position.x + tolerance >= _mapCoordinates.RightSideBorder;
            _isNearOrMoreLeft = _targetTransform.position.x - tolerance <= _mapCoordinates.LeftSideBorder;
            _isNearOrMoreUp = _targetTransform.position.y + tolerance >= _mapCoordinates.UpSideBorder;
            _isNearOrMoreDown = _targetTransform.position.y - tolerance <= _mapCoordinates.DownSideBorder;
        }

        private bool TryCornerTeleport()
        {
            UpdateNearFlags(_cornerTolerance);

            if (!(_isNearOrMoreUp || _isNearOrMoreRight || _isNearOrMoreLeft || _isNearOrMoreDown))
            {
                return false;
            }

            ChangeTargetTransform();
            return true;
        }


        private void ChangeTargetTransform()
        {
            var newPosition = _targetTransform.position;
            if (_isNearOrMoreRight)
            {
                newPosition.x = _mapCoordinates.LeftSideBorder + _teleportOffset;
            }

            if (_isNearOrMoreLeft)
            {
                newPosition.x = _mapCoordinates.RightSideBorder - _teleportOffset;
            }

            if (_isNearOrMoreUp)
            {
                newPosition.y = _mapCoordinates.DownSideBorder + _teleportOffset;
            }

            if (_isNearOrMoreDown)
            {
                newPosition.y = _mapCoordinates.UpSideBorder - _teleportOffset;
            }

            _targetTransform.position = newPosition;
        }
    }
}