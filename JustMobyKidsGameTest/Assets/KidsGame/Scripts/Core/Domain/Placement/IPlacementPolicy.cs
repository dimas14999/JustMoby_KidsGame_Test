using UnityEngine;

namespace KidsGame.Scripts.Core.Domain.Placement
{
    public interface IPlacementPolicy
    {
        PlacementDecision Evaluate(PlacementRequest request);
    }

    public struct PlacementRequest
    {
        public bool IsFirstCube;
        public Vector2 ZoneLocalPoint;
        public Vector2 TopCenter;
        public Vector2 TopSize;
        public Vector2 NewCubeSize;
        public Rect ZoneRect;
        public float StackSnapRadius;
        public float PlacementHeightFactor;
    }

    public struct PlacementDecision
    {
        public bool IsValid;
        public bool IsHeightLimit;
        public Vector2 DropPosition;
    }
}

