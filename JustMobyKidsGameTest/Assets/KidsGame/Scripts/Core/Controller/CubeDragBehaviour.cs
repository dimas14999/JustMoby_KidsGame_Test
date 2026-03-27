using KidsGame.Scripts.View;
using KidsGame.Scripts.Core.Application.Drop;
using KidsGame.Scripts.Core.Domain.Placement;
using KidsGame.Scripts.Config;
using KidsGame.Scripts.Infrastructure.Services.Animation;
using KidsGame.Scripts.Infrastructure.Services.Coordinates;
using KidsGame.Scripts.Infrastructure.Services.Feedback;
using KidsGame.Scripts.Infrastructure.Services.Localization;
using KidsGame.Scripts.Infrastructure.Services.Tower;
using UnityEngine;
using UnityEngine.EventSystems;
using System;

namespace KidsGame.Scripts.Core.Controller
{
    public class CubeDragBehaviour : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
    {
        private enum RejectReason
        {
            None,
            InvalidPlacement,
            HeightLimit
        }

        private CubeView _cube;
        private RectTransform _zone;
        private HoleZoneView _holeZone;
        private IActionFeedbackService _actionFeedback;
        private IPlacementPolicy _placementPolicy;
        private ITowerCollapseAnimationService _collapseAnimationService;
        private IHoleFallAnimationService _holeFallAnimationService;
        private ICubePlaceAnimationService _placeAnimationService;
        private ICubeMissAnimationService _missAnimationService;
        private IDragCoordinateService _dragCoordinateService;
        private IZoneClampService _zoneClampService;
        private ITowerQueryService _towerQueryService;
        private IDropDecisionService _dropDecisionService;
        private DragRulesConfig _dragRulesConfig;
        private Action _onStateChanged;
        private RectTransform _dragRoot;
        private Canvas _zoneCanvas;

        private Camera _dragCamera;
        private bool _dragging;
        private bool _createdFromPalette;

        private RectTransform _returnParent;
        private Vector2 _returnAnchoredPosition;
        private RejectReason _lastRejectReason;

        public void Init(CubeView cube, RectTransform zone, HoleZoneView holeZone, IActionFeedbackService actionFeedback,
            IPlacementPolicy placementPolicy, ITowerCollapseAnimationService collapseAnimationService,
            IHoleFallAnimationService holeFallAnimationService,
            ICubePlaceAnimationService placeAnimationService,
            ICubeMissAnimationService missAnimationService,
            IDragCoordinateService dragCoordinateService,
            IZoneClampService zoneClampService,
            ITowerQueryService towerQueryService,
            IDropDecisionService dropDecisionService,
            DragRulesConfig dragRulesConfig,
            Action onStateChanged,
            bool createdFromPalette = false)
        {
            _cube = cube;
            _zone = zone;
            _holeZone = holeZone;
            _actionFeedback = actionFeedback;
            _placementPolicy = placementPolicy;
            _collapseAnimationService = collapseAnimationService;
            _holeFallAnimationService = holeFallAnimationService;
            _placeAnimationService = placeAnimationService;
            _missAnimationService = missAnimationService;
            _dragCoordinateService = dragCoordinateService;
            _zoneClampService = zoneClampService;
            _towerQueryService = towerQueryService;
            _dropDecisionService = dropDecisionService;
            _dragRulesConfig = dragRulesConfig;
            _onStateChanged = onStateChanged;
            _zoneCanvas = _zone != null ? _zone.GetComponentInParent<Canvas>() : null;
            _createdFromPalette = createdFromPalette;
            _lastRejectReason = RejectReason.None;
        }

        public void OnPointerDown(PointerEventData eventData) => 
            BeginExternalDrag(eventData.position, eventData.pressEventCamera);

        public void OnDrag(PointerEventData eventData) => 
            DragExternal(eventData.position, eventData.pressEventCamera);

        public void OnPointerUp(PointerEventData eventData) => 
            EndExternalDrag(eventData.position, eventData.pressEventCamera);

        public void BeginExternalDrag(Vector2 screenPosition, Camera eventCamera)
        {
            if (_cube == null) return;
            if (_zone == null) return;

            _dragging = true;
            _dragCamera = ResolveDragCamera(eventCamera);

            _returnParent = _cube.Rect.parent as RectTransform;
            _returnAnchoredPosition = _cube.Rect.anchoredPosition;
            _dragRoot = _dragCoordinateService.GetDragRoot(_zone);
            if (_dragRoot == null) return;

            _cube.Rect.SetParent(_dragRoot, false);
            NormalizeRectForPointerPlacement(_cube.Rect);

            SetCubePositionFromScreenPoint(screenPosition);
        }

        public void DragExternal(Vector2 screenPosition, Camera eventCamera)
        {
            if (!_dragging) return;
            if (_dragCamera == null) _dragCamera = ResolveDragCamera(eventCamera);
            SetCubePositionFromScreenPoint(screenPosition);
        }
        
        public void EndExternalDrag(Vector2 screenPosition, Camera eventCamera)
        {
            if (!_dragging) return;

            if (!_createdFromPalette)
            {
                var existingDecision = _dropDecisionService.DecideForExistingCube(IsOverHole(screenPosition, eventCamera));
                if (existingDecision.Outcome == DropOutcomeType.DestroyInHole)
                {
                    var removedHeight = _towerQueryService.ResolveCubeHeight(_zone, _cube);
                    var collapseDone = _collapseAnimationService == null;
                    var fallDone = false;
                    void TryFinish()
                    {
                        if (collapseDone && fallDone)
                            _onStateChanged?.Invoke();
                    }

                    ShowAction(LocalizationKeys.CubeDestroyedInHole);

                    _collapseAnimationService?.CollapseAfterRemoval(_zone, _cube.transform,
                        _returnAnchoredPosition.y, removedHeight, () =>
                        {
                            collapseDone = true;
                            TryFinish();
                        });

                    PlayFallIntoHoleAndDestroy(eventCamera, () =>
                    {
                        fallDone = true;
                        TryFinish();
                    });
                }
                else
                {
                    _cube.Rect.SetParent(_returnParent, false);
                    NormalizeRectForPointerPlacement(_cube.Rect);
                    _cube.Rect.anchoredPosition = _returnAnchoredPosition;
                }

                _dragging = false;
                _dragCamera = null;
                return;
            }

            var zoneCamera = ResolveDragCamera(eventCamera);
            var insideZone = TryGetZoneLocalPoint(screenPosition, zoneCamera, out var zoneLocalPoint) &&
                             _zone.rect.Contains(zoneLocalPoint);

            var canPlace = TryGetDropPositionInZone(zoneLocalPoint, out var dropPosition);
            var isHeightLimit = _lastRejectReason == RejectReason.HeightLimit;
            var newDecision = _dropDecisionService.DecideForNewCube(insideZone, canPlace, isHeightLimit, dropPosition);
            if (newDecision.Outcome == DropOutcomeType.PlaceInZone)
            {
                _cube.Rect.SetParent(_zone, false);
                NormalizeRectForPointerPlacement(_cube.Rect);
                _cube.Rect.anchoredPosition = newDecision.DropPosition;
                ClampToZone();
                var finalPosition = _cube.Rect.anchoredPosition;
                _createdFromPalette = false;
                PlayPlaceAnimation(finalPosition, () =>
                {
                    ShowAction(LocalizationKeys.CubePlaced);
                    _onStateChanged?.Invoke();
                });
            }
            else
            {
                _lastRejectReason = newDecision.Outcome == DropOutcomeType.MissHeightLimit
                    ? RejectReason.HeightLimit
                    : RejectReason.InvalidPlacement;
                PlayMissBounceThenDestroy();
                _dragging = false;
                _dragCamera = null;
                return;
            }

            _dragging = false;
            _dragCamera = null;
        }

        private void PlayMissBounceThenDestroy()
        {
            if (_cube == null) return;

            var cubeGo = _cube.gameObject;
            ShowAction(_lastRejectReason == RejectReason.HeightLimit
                ? LocalizationKeys.HeightLimitReached
                : LocalizationKeys.CubeRemoved);
            if (_missAnimationService == null)
            {
                if (cubeGo != null)
                    Destroy(cubeGo);
                return;
            }

            _missAnimationService.PlayAndDestroy(_cube.Rect, () =>
            {
                if (cubeGo != null)
                    Destroy(cubeGo);
            });
        }

        private Camera ResolveDragCamera(Camera eventCamera) => 
            _dragCoordinateService.ResolveCanvasCamera(_zoneCanvas, eventCamera);

        private void SetCubePositionFromScreenPoint(Vector2 screenPosition)
        {
            if (!_dragging) return;
            if (_cube == null || _dragRoot == null) return;

            var ok = _dragCoordinateService.TryScreenToLocal(_dragRoot, screenPosition, _dragCamera, out var localPoint);
            if (!ok)
                return;

            _cube.Rect.anchoredPosition = localPoint;
        }

        private static void NormalizeRectForPointerPlacement(RectTransform rectTransform)
        {
            var w = rectTransform.rect.width;
            var h = rectTransform.rect.height;
            rectTransform.anchorMin = rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
            rectTransform.pivot = new Vector2(0.5f, 0.5f);
            rectTransform.sizeDelta = new Vector2(w, h);
        }

        private bool TryGetZoneLocalPoint(Vector2 screenPosition, Camera zoneCamera, out Vector2 localPoint)
        {
            if (_zone != null)
                return _dragCoordinateService.TryScreenToLocal(_zone, screenPosition, zoneCamera, out localPoint);
            localPoint = default;
            return false;
        }

        private void ClampToZone() => 
            _cube.Rect.anchoredPosition = _zoneClampService.Clamp(_zone, _cube.Rect, _cube.Rect.anchoredPosition);

        private bool TryGetDropPositionInZone(Vector2 zoneLocalPoint, out Vector2 dropPosition)
        {
            dropPosition = zoneLocalPoint;
            _lastRejectReason = RejectReason.None;

            if (!_createdFromPalette)
                return true;

            var hasOtherCubes = _towerQueryService.TryGetTopCube(_zone, _cube.transform, out var topCube);
            if (!hasOtherCubes || topCube == null)
                return true;

            var decision = _placementPolicy.Evaluate(new PlacementRequest
            {
                IsFirstCube = false,
                ZoneLocalPoint = zoneLocalPoint,
                TopCenter = topCube.Rect.anchoredPosition,
                TopSize = topCube.Rect.rect.size,
                NewCubeSize = _cube.Rect.rect.size,
                ZoneRect = _zone.rect,
                StackSnapRadius = _dragRulesConfig.StackSnapRadius,
                PlacementHeightFactor = _dragRulesConfig.PlacementHeightFactor
            });

            if (!decision.IsValid)
            {
                _lastRejectReason = decision.IsHeightLimit ? RejectReason.HeightLimit : RejectReason.InvalidPlacement;
                return false;
            }

            dropPosition = decision.DropPosition;
            return true;
        }

        private bool IsOverHole(Vector2 screenPosition, Camera eventCamera)
        {
            if (_holeZone == null || _holeZone.Zone == null) return false;
            return _dragCoordinateService.IsInsideRect(_holeZone.Zone, screenPosition, eventCamera);
        }

        private void PlayFallIntoHoleAndDestroy(Camera eventCamera, Action onComplete)
        {
            if (_cube == null || _dragRoot == null) return;

            if (!TryGetHoleCenterInDragRootLocal(eventCamera, out var targetLocal))
                targetLocal = _cube.Rect.anchoredPosition;

            var rect = _cube.Rect;
            var cubeGo = _cube.gameObject;
            if (_holeFallAnimationService == null)
            {
                Destroy(cubeGo);
                onComplete?.Invoke();
                return;
            }

            _holeFallAnimationService.Play(rect, targetLocal, () =>
            {
                if (cubeGo != null)
                    Destroy(cubeGo);
                onComplete?.Invoke();
            });
        }

        private bool TryGetHoleCenterInDragRootLocal(Camera eventCamera, out Vector2 localInDragRoot)
        {
            localInDragRoot = default;
            if (_holeZone.Zone == null || _dragRoot == null) return false;
            return _dragCoordinateService.TryGetHoleCenterInDragRootLocal(
                _dragRoot, _holeZone, _dragCamera, eventCamera, out localInDragRoot);
        }

        private void PlayPlaceAnimation(Vector2 dropPosition, Action onComplete)
        {
            if (_cube == null) return;
            if (_placeAnimationService == null)
            {
                _cube.Rect.anchoredPosition = dropPosition;
                onComplete?.Invoke();
                return;
            }

            _placeAnimationService.Play(_cube.Rect, dropPosition, onComplete);
        }

        private void ShowAction(string messageKey)
        {
            if (_actionFeedback == null || _cube == null) return;
            _actionFeedback.Show(messageKey, _cube.CubeColor);
        }

        private void OnDestroy()
        {
            if (!_dragging || _cube == null) return;
            _dragging = false;
            _dragCamera = null;
        }
    }
}

