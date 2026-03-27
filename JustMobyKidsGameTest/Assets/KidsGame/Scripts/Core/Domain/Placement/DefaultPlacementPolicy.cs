using UnityEngine;

namespace KidsGame.Scripts.Core.Domain.Placement
{
    public class DefaultPlacementPolicy : IPlacementPolicy
    {
        private const float HalfFactor = 0.5f;
        private const float SnapRadiusFraction = 0.25f;
        private const float HeightTolerance = 0.001f;

        public PlacementDecision Evaluate(PlacementRequest request)
        {
            if (request.IsFirstCube)
                return new PlacementDecision { IsValid = true, DropPosition = request.ZoneLocalPoint };

            var topHalfH = request.TopSize.y * HalfFactor;
            var topHalfW = request.TopSize.x * HalfFactor;
            var topSurfaceY = request.TopCenter.y + topHalfH;

            var minAllowedY = topSurfaceY;
            var maxAllowedY = topSurfaceY + request.NewCubeSize.y * request.PlacementHeightFactor;
            var insideHeightWindow = request.ZoneLocalPoint.y >= minAllowedY && request.ZoneLocalPoint.y <= maxAllowedY;
            var nearTopByX = Mathf.Abs(request.ZoneLocalPoint.x - request.TopCenter.x)
                             <= topHalfW + request.StackSnapRadius * SnapRadiusFraction;

            if (!insideHeightWindow || !nearTopByX)
                return new PlacementDecision { IsValid = false };

            var topY = request.TopCenter.y + topHalfH;
            var newHalfH = request.NewCubeSize.y * HalfFactor;
            var maxHorizontalOffset = request.TopSize.x * HalfFactor;
            var randomHorizontalOffset = Random.Range(-maxHorizontalOffset, maxHorizontalOffset);
            var dropPosition = new Vector2(request.TopCenter.x + randomHorizontalOffset, topY + newHalfH);

            var minX = request.ZoneRect.xMin + request.NewCubeSize.x * HalfFactor;
            var maxX = request.ZoneRect.xMax - request.NewCubeSize.x * HalfFactor;
            var minY = request.ZoneRect.yMin + request.NewCubeSize.y * HalfFactor;
            var maxY = request.ZoneRect.yMax - request.NewCubeSize.y * HalfFactor;

            var fit = dropPosition.x >= minX && dropPosition.x <= maxX
                      && dropPosition.y >= minY && dropPosition.y <= maxY;

            if (!fit)
                return new PlacementDecision
                {
                    IsValid = false,
                    IsHeightLimit = dropPosition.y > maxY + HeightTolerance
                };

            return new PlacementDecision { IsValid = true, DropPosition = dropPosition };
        }
    }
}
