using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using KidsGame.Scripts.Core.Controller;
using KidsGame.Scripts.Config;
using Zenject;

namespace KidsGame.Scripts.View
{
    public class UICubeView : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        [SerializeField] private Image _image = null!;

        [Inject] private ICubeSelectionChannel _selectionChannel;
        [Inject] private DragRulesConfig _dragRulesConfig;

        private BoxData _data;
        private float _pointerDownTime;
        private int _activePointerId = int.MinValue;
        private bool _spawnDragAccepted;
        private bool _forwardDragToScroll;
        private ScrollRect _scrollRect;

        public void Init(BoxData data)
        {
            _data = data;
            _image.sprite = data.Sprite;
        }

        private void Awake()
        {
            if (_scrollRect == null)
                _scrollRect = GetComponentInParent<ScrollRect>();
        }

        private void OnDisable()
        {
            if (_scrollRect != null)
                _scrollRect.enabled = true;
            ResetGestureState();
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            _activePointerId = eventData.pointerId;
            _pointerDownTime = Time.unscaledTime;
            _spawnDragAccepted = false;
            _forwardDragToScroll = false;
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (_data == null) return;
            if (eventData.pointerId != _activePointerId) return;
            if (Time.unscaledTime - _pointerDownTime < _dragRulesConfig.SpawnHoldDelaySeconds)
            {
                _spawnDragAccepted = false;
                _forwardDragToScroll = true;
                _scrollRect.OnBeginDrag(eventData);
                return;
            }

            _spawnDragAccepted = true;
            _forwardDragToScroll = false;
            if (_scrollRect != null) _scrollRect.enabled = false;
            _selectionChannel.Publish(new CubePointerEvent(
                CubePointerEventType.BeginDrag,
                _data,
                eventData.position,
                eventData.pressEventCamera,
                eventData.pointerId));
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (_data == null) return;
            if (eventData.pointerId != _activePointerId) return;
            if (_forwardDragToScroll)
            {
                _scrollRect.OnDrag(eventData);
                return;
            }

            if (!_spawnDragAccepted) return;
            _selectionChannel.Publish(new CubePointerEvent(
                CubePointerEventType.Drag,
                _data,
                eventData.position,
                eventData.pressEventCamera,
                eventData.pointerId));
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (_data == null) return;
            if (eventData.pointerId != _activePointerId) return;
            if (_forwardDragToScroll)
            {
                _scrollRect.OnEndDrag(eventData);
                ResetGestureState();
                return;
            }

            if (!_spawnDragAccepted)
            {
                ResetGestureState();
                return;
            }
            _selectionChannel.Publish(new CubePointerEvent(
                CubePointerEventType.EndDrag,
                _data,
                eventData.position,
                eventData.pressEventCamera,
                eventData.pointerId));
            if (_scrollRect != null)
                _scrollRect.enabled = true;
            ResetGestureState();
        }

        private void ResetGestureState()
        {
            _activePointerId = int.MinValue;
            _spawnDragAccepted = false;
            _forwardDragToScroll = false;
        }
    }
}