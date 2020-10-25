using UnityEngine;

namespace EnemyController
{
    public class StartForceGenerator
    {
        private readonly MapCoordinates _mapCoordinates;
        private float _minForce;
        private float _maxForce;

        public StartForceGenerator(MapCoordinates mapCoordinates, float minForce, float maxForce)
        {
            _minForce = minForce;
            _maxForce = maxForce;
            _mapCoordinates = mapCoordinates;
        }

        public Vector3 GetStartForce()
        {
            var directionPointX = Random.Range(_mapCoordinates.LeftSideBorder, _mapCoordinates.RightSideBorder);
            var directionPointY = Random.Range(_mapCoordinates.DownSideBorder, _mapCoordinates.UpSideBorder);
            var directionVector = new Vector3(directionPointX, directionPointY).normalized;
            return directionVector * Random.Range(_minForce, _maxForce);
        }
    }
}