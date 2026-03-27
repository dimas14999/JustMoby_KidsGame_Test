using DG.Tweening;
using KidsGame.Scripts.View;
using UnityEngine;
using KidsGame.Scripts.Config;

namespace KidsGame.Scripts.Infrastructure.Services.Animation
{
    public class TowerCollapseAnimationService : ITowerCollapseAnimationService
    {
        private const float MinHeightThreshold = 0.001f;
        
        private readonly AnimationConfig _config;
       
        public TowerCollapseAnimationService(AnimationConfig config)
        {
            _config = config;
        }

        public void CollapseAfterRemoval(
            RectTransform zone,
            Transform removedTransform,
            float removedCenterY,
            float removedHeight,
            System.Action onComplete)
        {
            if (zone == null || removedHeight <= MinHeightThreshold)
            {
                onComplete?.Invoke();
                return;
            }

            var animationsRemaining = 0;
            var delayIndex = 0;

            for (var i = 0; i < zone.childCount; i++)
            {
                var child = zone.GetChild(i);
                if (child == removedTransform) continue;

                var cube = child.GetComponent<CubeView>();
                if (cube == null) continue;

                var rect = cube.Rect;
                if (rect == null) continue;

                if (rect.anchoredPosition.y <= removedCenterY) continue;

                animationsRemaining++;
                AnimateCube(rect, removedHeight, delayIndex++, () =>
                {
                    animationsRemaining--;
                    if (animationsRemaining == 0)
                        onComplete?.Invoke();
                });
            }

            if (animationsRemaining == 0)
                onComplete?.Invoke();
        }

        private void AnimateCube(RectTransform rect, float removedHeight, int delayIndex, System.Action onComplete)
        {
            var targetY = rect.anchoredPosition.y - removedHeight;

            rect.DOKill();

            DOTween.Sequence()
                .Append(rect.DOAnchorPosY(targetY, _config.CollapseDuration)
                    .SetDelay(delayIndex * _config.CollapseStepDelay)
                    .SetEase(Ease.OutCubic))
                .Append(rect.DOAnchorPosY(targetY + _config.CollapseBounceHeight,
                        _config.CollapseBounceDuration)
                    .SetEase(Ease.OutQuad))
                .Append(rect.DOAnchorPosY(targetY,
                        _config.CollapseBounceDuration )
                    .SetEase(Ease.OutBounce))
                .OnComplete(() => onComplete?.Invoke());
        }
    }
}

