using System;
using DG.Tweening;
using UnityEngine;
using KidsGame.Scripts.Config;

namespace KidsGame.Scripts.Infrastructure.Services.Animation
{
    public class CubePlaceAnimationService : ICubePlaceAnimationService
    {
        private readonly AnimationConfig _config;

        public CubePlaceAnimationService(AnimationConfig config)
        {
            _config = config;
        }

        public void Play(RectTransform cubeRect, Vector2 dropPosition, Action onComplete)
        {
            if (cubeRect == null) return;

            cubeRect.DOKill();
            var startPosition = new Vector2(dropPosition.x, dropPosition.y + _config.PlaceJumpHeight);
            var randomTilt = UnityEngine.Random.Range(-_config.PlaceTiltAngle, _config.PlaceTiltAngle);

            cubeRect.anchoredPosition = startPosition;
            cubeRect.localScale = new Vector3(0.9f, 1.12f, 1f);
            cubeRect.localRotation = Quaternion.Euler(0f, 0f, randomTilt);

            var sequence = DOTween.Sequence();
            sequence.Append(cubeRect.DOAnchorPos(dropPosition, _config.PlaceDropDuration).SetEase(Ease.InQuad));
            sequence.Join(cubeRect.DOScale(new Vector3(1.14f, 0.86f, 1f), _config.PlaceDropDuration).SetEase(Ease.InQuad));
            sequence.Join(cubeRect.DORotate(Vector3.zero, _config.PlaceDropDuration).SetEase(Ease.OutSine));
            sequence.Append(cubeRect.DOScale(Vector3.one, _config.PlaceReboundDuration).SetEase(Ease.OutBack));
            sequence.Join(cubeRect.DOAnchorPosY(dropPosition.y + _config.PlaceReboundOffsetY, _config.PlaceReboundDuration * 0.45f)
                .SetLoops(2, LoopType.Yoyo)
                .SetEase(Ease.OutQuad));

            sequence.OnComplete(() =>
            {
                if (cubeRect != null)
                    cubeRect.anchoredPosition = dropPosition;
                onComplete?.Invoke();
            });
        }
    }
}

