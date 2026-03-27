using KidsGame.Scripts.View;
using UnityEngine;

namespace KidsGame.Scripts.Infrastructure.Services.Tower
{
    public class TowerQueryService : ITowerQueryService
    {
        private const float MinValidHeight = 0.001f;
        
        public bool TryGetTopCube(RectTransform zone, Transform excluded, out CubeView topCube)
        {
            topCube = null;
            if (zone == null) return false;

            var maxY = float.MinValue;
            for (var i = 0; i < zone.childCount; i++)
            {
                var child = zone.GetChild(i);
                if (child == excluded) continue;

                var childCube = child.GetComponent<CubeView>();
                if (childCube == null) continue;

                var childCubeY = childCube.Rect.anchoredPosition.y;
                if (childCubeY <= maxY) continue;
                maxY = childCubeY;
                topCube = childCube;
            }

            return topCube != null;
        }

        public float ResolveCubeHeight(RectTransform zone, CubeView currentCube)
        {
            var height = currentCube != null ? currentCube.Rect.rect.height : 0f;
            if (height > MinValidHeight) return height;
            if (zone == null) return 0f;

            for (var i = 0; i < zone.childCount; i++)
            {
                var childCube = zone.GetChild(i).GetComponent<CubeView>();
                if (childCube == null) continue;
                var h = childCube.Rect.rect.height;
                if (h > MinValidHeight) return h;
            }

            return 0f;
        }
    }
}

