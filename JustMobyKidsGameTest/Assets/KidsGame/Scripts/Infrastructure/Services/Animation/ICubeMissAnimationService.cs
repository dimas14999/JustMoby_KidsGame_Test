using System;
using UnityEngine;

namespace KidsGame.Scripts.Infrastructure.Services.Animation
{
    public interface ICubeMissAnimationService
    {
        void PlayAndDestroy(RectTransform cubeRect, Action onComplete);
    }
}

