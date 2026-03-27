using System;
using DG.Tweening;
using UnityEngine;
using KidsGame.Scripts.Config;

namespace KidsGame.Scripts.Infrastructure.Services.Animation
{
    public class HoleFallAnimationService : IHoleFallAnimationService
    {
        private readonly AnimationConfig _config;

        public HoleFallAnimationService(AnimationConfig config)
        {
            _config = config;
        }

        public void Play(RectTransform cubeRect, Vector2 targetLocal, Action onComplete)
        {
            if (cubeRect == null)
            {
                onComplete?.Invoke();
                return;
            }

            cubeRect.DOKill();

            var startPos = cubeRect.anchoredPosition;
            var direction = (targetLocal - startPos);
            if (direction.sqrMagnitude < 1f)
                direction = Vector2.down;
            direction.Normalize();
            var position = startPos + direction * _config.HoleOffset;

            var spin = UnityEngine.Random.Range(_config.HoleSpinMin, _config.HoleSpinMax) * (UnityEngine.Random.value > 0.5f ? 1f : -1f);

            var sequence = DOTween.Sequence();
            sequence.Append(cubeRect.DOAnchorPos(position, _config.HoleDuration).SetEase(Ease.OutQuad));
            sequence.Append(cubeRect.DOAnchorPos(targetLocal, _config.HoleFallDuration).SetEase(Ease.InCubic));
            sequence.Join(cubeRect.DOScale(Vector3.zero, _config.HoleFallDuration * 0.92f).SetEase(Ease.InCubic));
            sequence.Join(cubeRect.DORotate(new Vector3(0f, 0f, spin), _config.HoleFallDuration, RotateMode.FastBeyond360)
                .SetEase(Ease.InCubic));
            sequence.OnComplete(() => onComplete?.Invoke());
        }
    }
}

