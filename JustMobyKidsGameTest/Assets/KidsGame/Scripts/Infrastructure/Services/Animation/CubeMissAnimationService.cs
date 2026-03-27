using System;
using DG.Tweening;
using UnityEngine;
using KidsGame.Scripts.Config;

namespace KidsGame.Scripts.Infrastructure.Services.Animation
{
    public class CubeMissAnimationService : ICubeMissAnimationService
    {
        private readonly AnimationConfig _config;

        public CubeMissAnimationService(AnimationConfig config)
        {
            _config = config;
        }

        public void PlayAndDestroy(RectTransform cubeRect, Action onComplete)
        {
            if (cubeRect == null)
            {
                onComplete?.Invoke();
                return;
            }

            cubeRect.DOKill();
            var sequence = DOTween.Sequence();
            sequence.Append(cubeRect.DOScale(Vector3.one * _config.MissBounceScale, _config.MissBouncePopDuration).SetEase(Ease.OutBack));
            sequence.Append(cubeRect.DOScale(Vector3.zero, _config.MissBounceShrinkDuration).SetEase(Ease.InBack));
            sequence.OnComplete(() => onComplete?.Invoke());
        }
    }
}

