using UnityEngine;

namespace PlayerController.Models
{
    public class MapCoordinates
    {
        private readonly Vector3 _upRightViewport = new Vector3(1, 1);
        private readonly Vector3 _downLeftViewport = new Vector3(0, 0);
        
        public readonly float RightSideBorder;
        public readonly float LeftSideBorder;
        public readonly float UpSideBorder;
        public readonly float DownSideBorder;

        public MapCoordinates(Camera mainCamera)
        {
            var upRightCorner = mainCamera.ViewportToWorldPoint(_upRightViewport);
            var downLeftCorner = mainCamera.ViewportToWorldPoint(_downLeftViewport);

            RightSideBorder = upRightCorner.x;
            UpSideBorder = upRightCorner.y;
            LeftSideBorder = downLeftCorner.x;
            DownSideBorder = downLeftCorner.y;
        }

        public bool IsOutOfMap(Vector3 position)
        {
            if (position.x > RightSideBorder || position.x < LeftSideBorder)
            {
                return true;
            }

            if (position.y > UpSideBorder || position.y < DownSideBorder)
            {
                return true;
            }

            return false;
        }
    }
}