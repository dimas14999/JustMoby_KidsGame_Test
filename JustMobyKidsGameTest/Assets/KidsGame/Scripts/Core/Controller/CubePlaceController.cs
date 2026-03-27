using KidsGame.Scripts.View;
using KidsGame.Scripts.Config;
using KidsGame.Scripts.Infrastructure.Factory;
using KidsGame.Scripts.Infrastructure.Services.Feedback;
using KidsGame.Scripts.Infrastructure.Services.State;
using DG.Tweening;
using UniRx;
using UnityEngine;
using Zenject;

namespace KidsGame.Scripts.Core.Controller
{
    public class CubePlaceController : MonoBehaviour
    {
        [SerializeField] private RectTransform _zone;
        [SerializeField] private HoleZoneView _holeZoneViewFallback;
        [SerializeField] private ActionTextView _actionTextView;

        [Inject] private ICubeFactory _cubeFactory;
        [Inject] private ICubeDragBehaviourFactory _cubeDragFactory;
        [Inject] private ICubeSelectionChannel _selectionChannel;
        [Inject] private ITowerStateService _towerStateService;
        [Inject] private AnimationConfig _animationConfig;
        [Inject] private IActionFeedbackService _actionFeedback;

        private readonly CompositeDisposable _disposables = new();
        private CubeDragBehaviour _activeDrag;
        private int _activePointerId = int.MinValue;

        private void Start()
        {
            _actionFeedback?.SetActionTextView(_actionTextView);
            _towerStateService.Restore(_zone, _cubeFactory, OnCubeRestored);

            _selectionChannel.Selection
                .Subscribe(HandlePointerEvent)
                .AddTo(_disposables);
        }

        private void HandlePointerEvent(CubePointerEvent pointerEvent)
        {
            switch (pointerEvent.Type)
            {
                case CubePointerEventType.BeginDrag:
                    BeginCreateAndDrag(pointerEvent);
                    break;
                case CubePointerEventType.Drag:
                    UpdateDrag(pointerEvent);
                    break;
                case CubePointerEventType.EndDrag:
                    EndDrag(pointerEvent);
                    break;
            }
        }

        private void BeginCreateAndDrag(CubePointerEvent pointerEvent)
        {
            _activePointerId = pointerEvent.PointerId;
            var cube = _cubeFactory.Create(_zone, pointerEvent.Data);
            _activeDrag = _cubeDragFactory.CreateForNew(cube, _zone, _holeZoneViewFallback, OnStateChanged);
            PlaySpawnBounce(cube);
            _activeDrag.BeginExternalDrag(pointerEvent.ScreenPosition, pointerEvent.EventCamera);
        }

        private void OnStateChanged() => 
            _towerStateService.Save(_zone);

        private void OnCubeRestored(CubeView cube) => 
            _cubeDragFactory.CreateForExisting(cube, _zone, _holeZoneViewFallback, OnStateChanged);

        private void PlaySpawnBounce(CubeView cube)
        {
            if (cube == null) return;
            var rect = cube.Rect;
            rect.DOKill();
            rect.localScale = Vector3.one * 0.9f;
            rect.DOScale(Vector3.one, _animationConfig.SpawnBounceDuration).SetEase(Ease.OutBack);
        }

        private void UpdateDrag(CubePointerEvent pointerEvent)
        {
            if (pointerEvent.PointerId != _activePointerId) return;
            if (_activeDrag == null) return;
            _activeDrag.DragExternal(pointerEvent.ScreenPosition, pointerEvent.EventCamera);
        }

        private void EndDrag(CubePointerEvent pointerEvent)
        {
            if (pointerEvent.PointerId != _activePointerId) return;
            if (_activeDrag != null)
                _activeDrag.EndExternalDrag(pointerEvent.ScreenPosition, pointerEvent.EventCamera);

            _activeDrag = null;
            _activePointerId = int.MinValue;
        }

        private void OnDestroy() => 
            _disposables.Dispose();
    }
}
