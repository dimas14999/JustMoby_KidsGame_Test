using UnityEngine;

namespace KidsGame.Scripts.Core.Application.Drop
{
    public interface IDropDecisionService
    {
        DropDecision DecideForExistingCube(bool isOverHole);
        DropDecision DecideForNewCube(bool isInsideZone, bool canPlace, bool isHeightLimit, Vector2 dropPosition);
    }
}

