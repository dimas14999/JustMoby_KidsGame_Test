using UnityEngine;
using KidsGame.Scripts.View;

namespace KidsGame.Scripts.Infrastructure.Services.Coordinates
{
    public interface IDragCoordinateService
    {
        RectTransform GetDragRoot(RectTransform zone);
        Camera ResolveCanvasCamera(Canvas canvas, Camera eventCamera);
        bool TryScreenToLocal(RectTransform target, Vector2 screenPosition, Camera camera, out Vector2 localPoint);
        bool IsInsideRect(RectTransform target, Vector2 screenPosition, Camera eventCamera);
        bool TryGetHoleCenterInDragRootLocal(RectTransform dragRoot, HoleZoneView holeZone, Camera dragCamera, Camera eventCamera, out Vector2 localInDragRoot);
    }
}

