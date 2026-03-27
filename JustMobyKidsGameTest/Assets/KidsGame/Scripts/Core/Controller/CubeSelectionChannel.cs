using System;
using KidsGame.Scripts.Config;
using UniRx;
using UnityEngine;

namespace KidsGame.Scripts.Core.Controller
{
    public interface ICubeSelectionChannel
    {
        IObservable<CubePointerEvent> Selection { get; }
        void Publish(CubePointerEvent pointerEvent);
    }

    public enum CubePointerEventType
    {
        BeginDrag,
        Drag,
        EndDrag
    }

    public readonly struct CubePointerEvent
    {
        public CubePointerEventType Type { get; }
        public BoxData Data { get; }
        public Vector2 ScreenPosition { get; }
        public Camera EventCamera { get; }
        public int PointerId { get; }

        public CubePointerEvent(CubePointerEventType type, BoxData data, Vector2 screenPosition, Camera eventCamera, int pointerId)
        {
            Type = type;
            Data = data;
            ScreenPosition = screenPosition;
            EventCamera = eventCamera;
            PointerId = pointerId;
        }
    }

    public class CubeSelectionChannel : ICubeSelectionChannel, IDisposable
    {
        private readonly Subject<CubePointerEvent> _selected = new();

        public IObservable<CubePointerEvent> Selection => _selected;

        public void Publish(CubePointerEvent pointerEvent)
        {
            if (pointerEvent.Data == null) return;
            _selected.OnNext(pointerEvent);
        }

        public void Dispose()
        {
            _selected.OnCompleted();
            _selected.Dispose();
        }
    }
}

