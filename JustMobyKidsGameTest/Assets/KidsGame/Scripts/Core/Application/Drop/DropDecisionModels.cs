using UnityEngine;

namespace KidsGame.Scripts.Core.Application.Drop
{
    public enum DropOutcomeType
    {
        ReturnToOrigin,
        DestroyInHole,
        PlaceInZone,
        MissInvalid,
        MissHeightLimit
    }

    public struct DropDecision
    {
        public DropOutcomeType Outcome;
        public Vector2 DropPosition;
    }
}

