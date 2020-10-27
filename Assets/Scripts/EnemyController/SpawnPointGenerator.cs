using UnityEngine;
using Random = UnityEngine.Random;

namespace EnemyController
{
    public class SpawnPointGenerator
    {
        private readonly MapCoordinates _mapCoordinates;
        private readonly float _offset;

        public SpawnPointGenerator(MapCoordinates mapCoordinates, float offset)
        {
            _mapCoordinates = mapCoordinates;
            _offset = offset;
        }

        /// <summary>
        /// Gets spawn point outside of map
        /// </summary>
        /// <returns></returns>
        public Vector3 GetSpawnPoint()
        {
            float minX = 0f, maxX = 0f, minY = 0f, maxY = 0f;

            var spawnArea = GetRandomSpawnArea();
            switch (spawnArea)
            {
                case SpawnArea.Up:
                case SpawnArea.Down:
                    minX = _mapCoordinates.LeftSideBorder - _offset;
                    maxX = _mapCoordinates.RightSideBorder + _offset;
                    break;
                case SpawnArea.Right:
                case SpawnArea.Left:
                    minY = _mapCoordinates.DownSideBorder - _offset;
                    maxY = _mapCoordinates.UpSideBorder + _offset;
                    break;
            }

            switch (spawnArea)
            {
                case SpawnArea.Up:
                    minY = _mapCoordinates.UpSideBorder + _offset / 2;
                    maxY = _mapCoordinates.UpSideBorder + _offset;
                    break;
                case SpawnArea.Right:
                    minX = _mapCoordinates.RightSideBorder + _offset / 2;
                    maxX = _mapCoordinates.RightSideBorder + _offset;
                    break;
                case SpawnArea.Down:
                    minY = _mapCoordinates.DownSideBorder - _offset;
                    maxY = _mapCoordinates.DownSideBorder - _offset / 2;
                    break;
                case SpawnArea.Left:
                    minX = _mapCoordinates.LeftSideBorder - _offset;
                    maxX = _mapCoordinates.LeftSideBorder - _offset / 2;
                    break;
            }

            return new Vector3(Random.Range(minX, maxX), Random.Range(minY, maxY));;
        }

        private SpawnArea GetRandomSpawnArea()
        {
            var randomNum = Random.Range(0f, 4f);

            if (randomNum < 1f)
            {
                return SpawnArea.Up;
            }

            if (randomNum < 2f)
            {
                return SpawnArea.Right;
            }

            if (randomNum < 3f)
            {
                return SpawnArea.Down;
            }

            return SpawnArea.Left;
        }
        
        private enum SpawnArea
        {
            Up,
            Right,
            Down,
            Left
        }
    }
}