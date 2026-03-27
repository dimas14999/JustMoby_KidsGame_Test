namespace KidsGame.Scripts.Core.Application.Drop
{
    public class DropDecisionService : IDropDecisionService
    {
        public DropDecision DecideForExistingCube(bool isOverHole)
        {
            return new DropDecision
            {
                Outcome = isOverHole ? DropOutcomeType.DestroyInHole : DropOutcomeType.ReturnToOrigin
            };
        }

        public DropDecision DecideForNewCube(bool isInsideZone, bool canPlace, bool isHeightLimit, UnityEngine.Vector2 dropPosition)
        {
            if (!isInsideZone)
            {
                return new DropDecision { Outcome = DropOutcomeType.MissInvalid };
            }

            if (canPlace)
            {
                return new DropDecision
                {
                    Outcome = DropOutcomeType.PlaceInZone,
                    DropPosition = dropPosition
                };
            }

            return new DropDecision
            {
                Outcome = isHeightLimit ? DropOutcomeType.MissHeightLimit : DropOutcomeType.MissInvalid
            };
        }
    }
}

