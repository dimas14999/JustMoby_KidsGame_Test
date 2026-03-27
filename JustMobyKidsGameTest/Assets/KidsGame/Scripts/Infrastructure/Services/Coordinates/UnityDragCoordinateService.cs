using KidsGame.Scripts.View;
using UnityEngine;

namespace KidsGame.Scripts.Infrastructure.Services.Coordinates
{
    public class UnityDragCoordinateService : IDragCoordinateService
    {
        public RectTransform GetDragRoot(RectTransform zone)
        {
            if (zone == null) return null;
            var canvas = zone.GetComponentInParent<Canvas>();
            if (canvas != null)
                return canvas.transform as RectTransform;
            return zone.parent as RectTransform;
        }

        public Camera ResolveCanvasCamera(Canvas canvas, Camera eventCamera)
        {
            if (canvas == null) return eventCamera != null ? eventCamera : Camera.main;
            if (canvas.renderMode == RenderMode.ScreenSpaceOverlay) return null;
            if (eventCamera != null) return eventCamera;
            if (canvas.worldCamera != null) return canvas.worldCamera;
            return Camera.main;
        }

        public bool TryScreenToLocal(RectTransform target, Vector2 screenPosition, Camera camera, out Vector2 localPoint)
        {
            if (target == null)
            {
                localPoint = default;
                return false;
            }

            return RectTransformUtility.ScreenPointToLocalPointInRectangle(target, screenPosition, camera, out localPoint);
        }

        public bool IsInsideRect(RectTransform target, Vector2 screenPosition, Camera eventCamera)
        {
            if (target == null) return false;
            var canvas = target.GetComponentInParent<Canvas>();
            var camera = ResolveCanvasCamera(canvas, eventCamera);
            return RectTransformUtility.RectangleContainsScreenPoint(target, screenPosition, camera);
        }

        public bool TryGetHoleCenterInDragRootLocal(
            RectTransform dragRoot,
            HoleZoneView holeZone,
            Camera dragCamera,
            Camera eventCamera,
            out Vector2 localInDragRoot)
        {
            localInDragRoot = default;
            if (dragRoot == null || holeZone == null || holeZone.Zone == null) return false;

            var holeRt = holeZone.Zone;
            var worldCenter = holeRt.TransformPoint(holeRt.rect.center);
            var holeCanvas = holeRt.GetComponentInParent<Canvas>();
            var projectionCamera = ResolveCanvasCamera(holeCanvas, eventCamera);

            if (projectionCamera == null && Camera.main != null)
                projectionCamera = Camera.main;
            if (projectionCamera == null) return false;

            var screenPoint = RectTransformUtility.WorldToScreenPoint(projectionCamera, worldCenter);
            return RectTransformUtility.ScreenPointToLocalPointInRectangle(dragRoot, screenPoint, dragCamera, out localInDragRoot);
        }
    }
}

