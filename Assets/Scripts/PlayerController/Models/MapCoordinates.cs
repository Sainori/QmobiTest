using UnityEngine;

namespace PlayerController.Models
{
    public class MapCoordinates
    {
        public readonly float RightSideBorder;
        public readonly float LeftSideBorder;
        public readonly float UpSideBorder;
        public readonly float DownSideBorder;

        public MapCoordinates(Vector3 upRightCorner, Vector3 downLeftCorner)
        {
            RightSideBorder = upRightCorner.x;
            UpSideBorder = upRightCorner.y;
            LeftSideBorder = downLeftCorner.x;
            DownSideBorder = downLeftCorner.y;
        }
    }
}