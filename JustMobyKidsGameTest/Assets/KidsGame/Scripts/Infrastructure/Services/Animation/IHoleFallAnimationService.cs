using System;
using UnityEngine;

namespace KidsGame.Scripts.Infrastructure.Services.Animation
{
    public interface IHoleFallAnimationService
    {
        void Play(RectTransform cubeRect, Vector2 targetLocal, Action onComplete);
    }
}

