using UnityEngine;
using System;

namespace KidsGame.Scripts.Infrastructure.Services.Animation
{
    public interface ICubePlaceAnimationService
    {
        void Play(RectTransform cubeRect, Vector2 dropPosition, Action onComplete);
    }
}

