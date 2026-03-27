using UnityEngine;

namespace KidsGame.Scripts.Infrastructure.Services.Coordinates
{
    public class ZoneClampService : IZoneClampService
    {
        public Vector2 Clamp(RectTransform zone, RectTransform cube, Vector2 desiredLocal)
        {
            if (zone == null || cube == null) return desiredLocal;

            var zoneRect = zone.rect;
            var cubeRect = cube.rect;
            var cubePivot = cube.pivot;

            var minX = zoneRect.xMin + cubeRect.width * cubePivot.x;
            var maxX = zoneRect.xMax - cubeRect.width * (1f - cubePivot.x);
            var minY = zoneRect.yMin + cubeRect.height * cubePivot.y;
            var maxY = zoneRect.yMax - cubeRect.height * (1f - cubePivot.y);
            if (minX > maxX) (minX, maxX) = (maxX, minX);
            if (minY > maxY) (minY, maxY) = (maxY, minY);

            desiredLocal.x = Mathf.Clamp(desiredLocal.x, minX, maxX);
            desiredLocal.y = Mathf.Clamp(desiredLocal.y, minY, maxY);
            return desiredLocal;
        }
    }
}

